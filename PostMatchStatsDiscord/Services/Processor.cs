using Discord.WebSocket;
using PostMatchStatsDiscord.Constants;
using PostMatchStatsDiscord.Models;

namespace PostMatchStatsDiscord.Services
{
    internal class Processor
    {
        private StratzService stratzService;
        private IdChecker idChecker;
        private MessageService messageService;

        public Processor(DiscordSocketClient client)
        {
            stratzService = new StratzService();
            idChecker = new IdChecker();
            messageService = new MessageService(client);
        }

        public async Task StartAsync()
        {
            while (true)
            {
                ProcessIdsAsync();
                var matches = await ProcessMatchesAsync();
                foreach (var match in matches)
                    await messageService.SendMessage(match);
                Thread.Sleep(12000);
            }
        }

        private async void ProcessIdsAsync()
        {
            var newIds = stratzService.GetLastMatchIdAsync().Result
                    .Where(id => !idChecker.IsObtained(id).Result);

            if (newIds.Any())
            {
                await JsonIO.AddIdsAsync(Paths.ObtainedMathesPath, newIds);
                await JsonIO.AddIdsAsync(Paths.NotParsedMatchesPath, newIds);
            }
        }

        private async Task<List<MatchStats>> ProcessMatchesAsync()
        {
            var matchesToParse = await JsonIO.ReadJsonAsync<IdHash>(Paths.NotParsedMatchesPath);

            if (!matchesToParse.Ids.Any())
                return new List<MatchStats>();

            var matches = matchesToParse.Ids
                .Select(id => stratzService.GetMatchByIdAsync(id).Result)
                .ToList();

            await JsonIO.DeleteNotParsedIdsAsync(
                matches
                .Where(match => match.ParsedDateTime is not null)
                .Select(m => m.Id));

            return matches
                .Where(match => match.ParsedDateTime is not null)
                .ToList();
        }
    }
}
