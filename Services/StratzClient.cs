using Contracts;
using Discord.Rest;
using Entities.Models;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StratzClient : IStratzClient
    {
        private readonly GraphQLHttpClient _client;

        public StratzClient()
        {
            _client = new GraphQLHttpClient("https://api.stratz.com/graphql", new SystemTextJsonSerializer());
            _client.HttpClient.DefaultRequestHeaders.Add(
                "Authorization", $"bearer {Environment.GetEnvironmentVariable("StratzAuthToken")}");
        }

        public async Task<IEnumerable<ChannelMatchesIds>> GetLastMatchesIdsAsync(IEnumerable<ChannelSubscribers> subs)
        {
            var idRequest = new GraphQLRequest
            {
                Query = $@"
                query GetLastPlayersMathes($playerIds: [Long]!){{
                  players(steamAccountIds: $playerIds){{
                    steamAccountId
                    matches(request: {{take: 1}}){{
                      id
                      statsDateTime
                    }}
	              }}	
                }}",
                Variables = new
                {
                    playerIds = subs.SelectMany(i => i.Subscribers).Distinct() // To fill with subscribed ids
                }
            };
            var response = await _client.SendQueryAsync<PreflightMatchId>(idRequest);

            var matchWithPlayer = response.Data.Players
                .SelectMany(p => p.Matches
                    .Where(m => m.ParsedDateTime is not null)
                    .Select(m => new { matchId = m.Id, playerId = p.SteamAccountId }));

            return subs.Select(s => 
                new ChannelMatchesIds
                {
                    ChannelId = s.ChannelId,
                    Matches = matchWithPlayer
                        .Where(m => s.Subscribers.Contains(m.playerId))
                        .Select(m => m.matchId)
                        .Distinct()
                });
        }

        public async Task<MatchStats> GetMatchByIdAsync(long id)
        {
            var statRequest = new GraphQLRequest
            {
                Query = $@"
                query GetMatchByIdAsync($matchId: Long!)    {{
                    match(id: $matchId) {{
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

            return (await _client.SendQueryAsync<MatchData>(statRequest)).Data.Match;
        }
    }
}
