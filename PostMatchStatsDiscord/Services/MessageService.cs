using Discord;
using Discord.WebSocket;
using PostMatchStatsDiscord.Models;

namespace PostMatchStatsDiscord.Services
{
    public class MessageService
    {
        private DiscordSocketClient _client;

        public MessageService(DiscordSocketClient client) => _client = client;


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
            embed.AddField($"` {match.DurationSeconds / 60}:{(match.DurationSeconds % 60).ToString().PadLeft(2, '0')} `", "‎ ", true);
            embed.AddField(GetEmote("Dire") + " - " + $"`{dire.Select(x => x.Stats.KillCount).Sum()}                     `", "‎ ", true);

            for (int i = 0; i < radiant.Length; i++)
            {
                AddStatsField(embed, radiant[i]);
                embed.AddField("‎ ", "‎ ", true);
                AddStatsField(embed, dire[i]);
            }

            AddRewardField(embed, match, "MVP", 7);
            AddRewardField(embed, match, "TOP_CORE", 10);
            AddRewardField(embed, match, "TOP_SUPPORT", 13);

            embed.Footer = new EmbedFooterBuilder().WithText("\npowered by Stratz");

            return embed.Build();
        }

        private void AddStatsField(EmbedBuilder embed, Player player)
        {
            embed.AddField(
                    GetEmote(GetRole(player.Position)) + " - "
                    + GetEmote(player.Hero.DisplayName)
                    + $" {(player.SteamAccount.Name.Length > 15 ? player.SteamAccount.Name.Substring(15) : player.SteamAccount.Name)}",
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
                    emoteName.Replace(" ", "").Replace("-", ""), StringComparison.OrdinalIgnoreCase) != -1);
    }
}
