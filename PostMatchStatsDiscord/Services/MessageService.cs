using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using NUnit.Framework.Constraints;
using PostMatchStatsDiscord.Constants;
using PostMatchStatsDiscord.Models;

namespace PostMatchStatsDiscord.Services
{
    public class MessageService
    {
        private DiscordSocketClient _client;

        public MessageService(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task SendMessage(MatchStats match)
        {
            var channel = await _client.GetChannelAsync(370233530346766338) as IMessageChannel;

            await channel.SendMessageAsync(embed: GetEmbed(match));
        }

        private Embed GetEmbed(MatchStats match)
        {
            var embed = new EmbedBuilder();

            embed.Color = match.DidRadiantWin ? Color.Green : Color.Red;

            var radiant = match.Players
                .Where(p => p.IsRadiant)
                .OrderBy(x => int.Parse(x.Position.Substring(x.Position.Length - 1, 1)))
                .ToArray();
            var dire = match.Players
                .Where(p => !p.IsRadiant)
                .OrderBy(x => int.Parse(x.Position.Substring(x.Position.Length - 1, 1)))
                .ToArray();

            embed.AddField($"`                    {radiant.Select(x => x.Stats.KillCount).Sum()}` - " + GetEmote("Radiant"), "‎ ", true);
            embed.AddField($"` {match.DurationSeconds / 60}:{match.DurationSeconds % 60} `", "‎ ", true);
            embed.AddField(GetEmote("Dire") + " - " + $"`{dire.Select(x => x.Stats.KillCount).Sum()}                     `", "‎ ", true);

            for (int i = 0; i < radiant.Length; i++)
            {
                embed.AddField(
                    GetEmote(GetRole(radiant[i].Position)) + " - " 
                    + GetEmote(radiant[i].Hero.DisplayName.Replace(" ", "")) 
                    + $" {(radiant[i].SteamAccount.Name.Length > 15 ? radiant[i].SteamAccount.Name.Substring(15) : radiant[i].SteamAccount.Name)}",
                    GetStats(radiant[i]), true);

                embed.AddField("‎ ", "‎ ", true);

                embed.AddField(
                    GetEmote(GetRole(dire[i].Position)) + " - "
                    + GetEmote(dire[i].Hero.DisplayName.Replace(" ", ""))
                    + $" {(dire[i].SteamAccount.Name.Length > 15 ? dire[i].SteamAccount.Name.Substring(15) : dire[i].SteamAccount.Name)}",
                    GetStats(dire[i]), true);
            }

            embed.Fields[7] = new EmbedFieldBuilder()
                .WithName("‎ ")
                .WithValue(
                    GetEmote("MVP") + " - "
                    + GetEmote(match.Players
                        .Where(p => p.Award == "MVP")
                        .Select(p => p.Hero.DisplayName.Replace(" ", ""))
                        .Single()))
                .WithIsInline(true);

            embed.Fields[10] = new EmbedFieldBuilder()
                .WithName("‎ ")
                .WithValue(
                    GetEmote("TopCore") + " - "
                    + GetEmote(match.Players
                        .Where(p => p.Award == "TOP_CORE")
                        .Select(p => p.Hero.DisplayName.Replace(" ", ""))
                        .Single()))
                .WithIsInline(true);

            embed.Fields[13] = new EmbedFieldBuilder()
                .WithName("‎ ")
                .WithValue(
                    GetEmote("TopSupport") + " - "
                    + GetEmote(match.Players
                        .Where(p => p.Award == "TOP_SUPPORT")
                        .Select(p => p.Hero.DisplayName.Replace(" ", ""))
                        .Single()))
                .WithIsInline(true);

            embed.Footer = new EmbedFooterBuilder().WithText("\npowered by Stratz");

            return embed.Build();
        }

        private string GetStats(Player player)
        {
            var kills = player.Stats.KillCount.ToString().PadLeft(2);
            var deaths = player.Stats.DeathCount.ToString().PadLeft(2);
            var assists = player.Stats.AssistCount.ToString().PadLeft(2);
            var imp = player.Imp.ToString().PadLeft(3);

            return $"`{kills}/{deaths}/{assists} imp:{imp} apm: {(int)player.Stats.ActionsPerMinute.Average()} `";
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
                    emoteName, StringComparison.OrdinalIgnoreCase) != -1);
        
    }
}
