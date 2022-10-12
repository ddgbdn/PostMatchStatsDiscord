using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using PostMatchStatsDiscord.Models;

namespace PostMatchStatsDiscord.Services
{
    internal class StratzService
    {
        public void GetLastMatch()
        {
            var client = new GraphQLHttpClient("https://api.stratz.com/graphql", new SystemTextJsonSerializer());
            client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {Environment.GetEnvironmentVariable("StratzAuthToken")}");
            var statRequest = new GraphQLRequest
            {
                Query = @"
                {
                  player(steamAccountId: 236888270) {
                    matches(request: {isParsed: true, take: 1}) {
    	                didRadiantWin,
                      id,
                      players {
                        steamAccount{
                          name
                          id
                        }
                        imp
                        award
                        hero {
                          displayName
                        },
                        isRadiant        
                      }      
                    }
                  }
                }"
            };
            var graphQlResponse = client.SendQueryAsync<Match>(statRequest);
            var ab = graphQlResponse.Result;
        }
    }
}
