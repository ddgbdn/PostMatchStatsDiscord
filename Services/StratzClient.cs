using Contracts;
using Discord.Rest;
using Entities.Models;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StratzClient : IStratzClient
    {
        private GraphQLHttpClient client;

        public StratzClient()
        {
            client = new GraphQLHttpClient("https://api.stratz.com/graphql", new SystemTextJsonSerializer());
            client.HttpClient.DefaultRequestHeaders.Add(
                "Authorization", $"bearer {Environment.GetEnvironmentVariable("StratzAuthToken")}");
        }

        public async Task<IEnumerable<long>> GetLastMatchesIdsAsync()
        {
            var idRequest = new GraphQLRequest
            {
                Query = $@"
                query GetLastPlayersMathes($playerIds: [Long]!){{
                  players(steamAccountIds: $playerIds){{
                    matches(request: {{take: 1}}){{
                      id
                      statsDateTime
                    }}
	              }}	
                }}",
                Variables = new
                {
                    playerIds = new long[] { 236888270 } // To fill with subscribed ids
                }
            };

            // TEMP
            // Player Ids with corresponding Guild Ids
            // Group by guild id and return tuple with needed channel id

            var response = await client.SendQueryAsync<PreflightMatchId>(idRequest);

            return response.Data.Players
                .SelectMany(p => p.Matches)
                .Where(m => m.ParsedDateTime is not null)
                .Select(m => m.Id)
                .Distinct();
        }

        public async Task<MatchStats> GetMatchByIdAsync(long id)
        {
            var statRequest = new GraphQLRequest
            {
                Query = $@"
                query GetMatchByIdAsync($matchId: Long!)    {{
                    match(id: 7137605986) {{
                        id
                        statsDateTime
                        didRadiantWin
                        durationSeconds
                        radiantKills
                        direKills
                        topLaneOutcome
                        midLaneOutcome
                        bottomLaneOutcome
                        players {{
                            steamAccount {{
                                id
                                name
                            }}
                            isRadiant
                            hero {{
                                displayName
                            }}
                            kills
                            deaths
                            assists
                            networth
                            heroDamage
                            position
                            imp
                            award
                            stats {{
                                actionsPerMinute
                            }}
                        }}
                    }}
                }}",
                Variables = new
                {
                    matchId = id
                }
            };

            return (await client.SendQueryAsync<MatchData>(statRequest)).Data.Match;
        }
    }
}
