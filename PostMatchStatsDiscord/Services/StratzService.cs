using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using PostMatchStatsDiscord.Constants;
using PostMatchStatsDiscord.Models;

namespace PostMatchStatsDiscord.Services
{
    public class StratzService
    {
        private GraphQLHttpClient client;

        public StratzService()
        {
            client = new GraphQLHttpClient("https://api.stratz.com/graphql", new SystemTextJsonSerializer());
            client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {Environment.GetEnvironmentVariable("StratzAuthToken")}");
        }

        public async Task<IEnumerable<long>> GetLastMatchIdAsync()
        {
            var idRequest = new GraphQLRequest
            {
                Query = $@"
                {{
                  players(steamAccountIds: [{PlayerIds.Okarin}, {PlayerIds.MoJungle}, {PlayerIds.Lymior}]){{
                    matches(request: {{take: 1}}){{
                      id
                    }}
                  }}
                }}"
            };
            var response = await client.SendQueryAsync<IdData>(idRequest);
            return response.Data.Players
                .SelectMany(x => x.Matches)
                .Select(m => m.Id)
                .Distinct();
        }

        public async Task<MatchStats> GetMatchByIdAsync(long id)
        {
            var statRequest = new GraphQLRequest
            {
                Query = $@"
                {{
                  match(id: {id}) {{
                    id
                    statsDateTime
                    didRadiantWin
                    durationSeconds
                    averageRank
                    players {{
                      steamAccount {{
                        id
                        name
                      }}
                      isRadiant
                      hero {{
                        displayName
                      }}
                      position
                      imp
                      award
                      stats {{
                        actionsPerMinute
                      }}
                    }}
                  }}
                }}"
            };
            var response = await client.SendQueryAsync<MatchData>(statRequest);
            return response.Data.Match;
        }
    }
}
