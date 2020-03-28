using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSimplify.Mappers;
using Flurl.Http;
using Flurl;
using RiotSimplify.Dto;
using System.Runtime.Caching;
using System.Collections.Concurrent;
using Dasync.Collections;

namespace RiotSimplify.Clients
{
    public class MatchClient
    {
        private String matchListEndpoint = "/lol/match/v4/matchlists/by-account/";
        private String uniqueMatchEndpoint = "/lol/match/v4/matches/";

        internal async Task<List<MatchResult>> GetMatchesBySeasonAsync(int seasonId, string queue)
        {
            MatchlistDto dto = null;
            ConcurrentBag<MatchResult> results = new ConcurrentBag<MatchResult>();

            if (String.IsNullOrEmpty(RiotApiUtils.AccountId))
            {
                throw new Exception("AccountId cannot be null or empty");
            }

            try
            {
                long seasonTimestamp = Utils.GetSeasonTimestamp(seasonId) * 1000; // to ms

                Url request = RiotApiUtils.Api
                        .AppendPathSegment(matchListEndpoint)
                        .AppendPathSegment(RiotApiUtils.AccountId)
                        .SetQueryParam("beginTime", seasonTimestamp)
                        .SetQueryParam("api_key", RiotApiUtils.ApiKey)
                        .SetQueryParam("queue", RiotApiUtils.GetQueueID(queue));

                bool completed = false;
                
                if (!request.QueryParams.ContainsKey("beginIndex"))
                {
                    request.SetQueryParam("beginIndex", 0);
                }

                do
                {
                    MatchlistDto current = await request.GetJsonAsync<MatchlistDto>();

                    if (current.Matches.Last().Timestamp <= seasonTimestamp || current.Matches.Count() < 100)
                    {
                        completed = true;
                    }

                    if (dto == null)
                    {
                        dto = current;
                    }
                    else
                    {
                        dto.Matches.AddRange(current.Matches);
                        dto.endIndex = current.endIndex;
                    }

                    request.SetQueryParam("beginIndex", (request.QueryParams["beginIndex"] as int?) + 100);

                } while (!completed);

                // Trim extra matches
                int removedMatches = dto.Matches.RemoveAll(m => m.Timestamp < seasonTimestamp);
                dto.endIndex -= removedMatches;

                try
                {
                    // WILL CAP OFF IN 100 REQUESTS 

                    await dto.Matches.ParallelForEachAsync(async match =>
                    {
                        var detail = await GetMatchResultAsync(match.GameId);
                        results.Add(detail);
                    }, maxDegreeOfParallelism: 20);

                    return results.ToList();
                }
                catch (Exception e)
                {
                    // IF WE HAVE MORE THAN 100 MATCHES
                   
                    return results.ToList();
                }
                
            }
            catch (Exception e)
            {
                throw e;
            }  
        }

        internal async Task<MatchResult> GetMatchResultAsync(long matchId, Dictionary<string, string> queryStringOptions = null)
        {
            Url url = RiotApiUtils.Api
                    .AppendPathSegment(uniqueMatchEndpoint)
                    .AppendPathSegment(matchId)
                    .SetQueryParam("api_key", RiotApiUtils.ApiKey)
                    .AppendQueryString(queryStringOptions);

            var detail = await url.GetJsonAsync<MatchDetailsDto>();

            return MatchResult.MapFrom(detail, RiotApiUtils.AccountId);
        }
    }
}
