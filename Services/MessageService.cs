using Contracts;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MessageService : IMessageService
    {
        private readonly DiscordSocketClient _client;

        public MessageService(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task SendMessage(MatchStats match, ulong channelId)
        {
            var channel = await _client.GetChannelAsync(channelId) as IMessageChannel;
            if (channel is null)
                return;

            await channel.SendMessageAsync(embed: BuildEmbed(match));
        }

        private Embed BuildEmbed(MatchStats match)
        {
            var embed = new EmbedBuilder();

            embed.Color = match.DidRadiantWin ? Color.Green : Color.Red;

            var radiant = GetPlayersByTeam(match, true);
            var dire = GetPlayersByTeam(match, false);

            AddHeaderFilds(embed, match, radiant, dire);

            for (int i = 0; i < radiant.Length; i++)
            {
                AddStatsField(embed, radiant[i]);
                embed.AddField("‎ ", "‎ ", true);
                AddStatsField(embed, dire[i]);
            }

            AddRewardField(embed, match, "MVP", 7);
            AddRewardField(embed, match, "TOP_CORE", 10);
            AddRewardField(embed, match, "TOP_SUPPORT", 13);

            AddFooterFields(embed, match);

            return embed.Build();
        }

        private void AddHeaderFilds(EmbedBuilder embed, MatchStats match, Player[] radiant, Player[] dire)
        {
            embed.AddField(
                match.DidRadiantWin ? "`WIN" : "`"
                + radiant.Sum(p => p.Kills).ToString().PadLeft(match.DidRadiantWin ? 23 : 26) + "`"
                + " - " + GetEmote("Radiant"),
                "‎ ",
                true);

            embed.AddField($"` {TimeSpan.FromSeconds(match.DurationSeconds):mm\\:ss} `", "‎ ", true);

            embed.AddField(
                GetEmote("Dire") + " - "
                + "`" + dire.Sum(p => p.Kills).ToString().PadRight(match.DidRadiantWin ? 26 : 23)
                + (match.DidRadiantWin ? "`" : "WIN`"),
                "‎ ",
                true);
        }

        private void AddStatsField(EmbedBuilder embed, Player player)
        {
            var kills = player.Kills.ToString();
            var deaths = player.Deaths.ToString();
            var assists = player.Assists.ToString();

            embed.AddField(
                    GetEmote(GetRole(player.Position)) + " - "
                    + GetEmote(player.Hero.DisplayName)
                    + $" {(player.SteamAccount.Name.Length > 11 ? player.SteamAccount.Name.Substring(0, 11) : player.SteamAccount.Name)}"
                    + " - " + $"`{kills}/{deaths}/{assists}`",
                    GetStats(player), true);
        }

        private void AddRewardField(EmbedBuilder embed, MatchStats match, string reward, int index)
        {
            embed.Fields[index] = new EmbedFieldBuilder()
                .WithName("‎ ")
                .WithValue(
                    GetEmote(reward.Replace("_", "")) + " - "
                    + GetEmote(match.Players
                        .Where(p => p.Award == reward)
                        .Select(p => p.Hero.DisplayName)
                        .Single()))
                .WithIsInline(true);
        }

        private void AddFooterFields(EmbedBuilder embed, MatchStats match)
        {
            embed.AddField("‎ ", "‎ ");

            var mid = $"Mid: {GetLaneOutcome(match.MidLaneOutcome)}";
            var top = $"Top: {GetLaneOutcome(match.TopLaneOutcome)}".PadRight((71 - mid.Length) / 2);
            var bot = $"Bot: {GetLaneOutcome(match.BottomLaneOutcome)}".PadLeft((71 - mid.Length) / 2);

            embed.AddField("` " + top + mid + bot + " `",
                "` Powered by STRATZ" + $"id: {match.Id} `".PadLeft(56));
        }

        private Player[] GetPlayersByTeam(MatchStats match, bool isRadiant)
            => match.Players
                .Where(p => p.IsRadiant == isRadiant)
                .OrderBy(x => int.Parse(x.Position.Substring(x.Position.Length - 1, 1)))
                .ToArray();

        private string GetStats(Player player)
        {
            var imp = player.Imp.ToString().PadLeft(3);
            var heroDamage = player.HeroDamage / 1000.0;
            var networth = player.Networth / 1000.0;

            return $"` imp:{imp} hd: {heroDamage.ToString("F1", CultureInfo.InvariantCulture)}k netw: {networth:F1}k `";
        }

        private string GetLaneOutcome(string outcome)
        {
            if (outcome == "TIE")
                return "Draw";

            var words = outcome.Split('_')
                .Select(w => w.Substring(0, 1) + w.Substring(1).ToLower());

            return string.Join(" ", words);
        }

        private string GetRole(string pos)
        {
            switch (pos)
            {
                case "POSITION_1":
                    return "Carry";
                case "POSITION_2":
                    return "Mid";
                case "POSITION_3":
                    return "Off";
                case "POSITION_4":
                    return "Soft";
                case "POSITION_5":
                    return "Hard";
                default:
                    return "Kapets";
            }
        }

        private GuildEmote? GetEmote(string emoteName)
            => _client.Guilds
                .SelectMany(x => x.Emotes)
                .FirstOrDefault(x => x.Name.IndexOf(
                    emoteName.Replace(" ", "").Replace("-", ""), StringComparison.OrdinalIgnoreCase) != -1);
    }
}

