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

        /// <summary>
        /// Throws 429 To Many Requests exception if the rate limit is exceeded. 
        /// Consider then specifiying the endIndex to receive less matches results
        /// </summary>
        /// <param name="seasonId"> Season which has the matches you want </param>
        /// <param name="queue"> Ladder from which the matches were played </param>
        /// <returns></returns>
        public async Task<List<MatchResult>> GetMatchesBySeasonAsync(int seasonId, string queue, int beginIndex = 0, int endIndex = 100, bool tooManyRequestsThrowException = true)
        {
            await RiotApiUtils.FetchSummonerData();

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
                        .SetQueryParam("queue", RiotApiUtils.GetQueueID(queue))
                        .SetQueryParam("beginIndex", beginIndex)
                        .SetQueryParam("endIndex", endIndex);

                Console.WriteLine(request.ToString());

                MatchlistDto dto = await request.GetJsonAsync<MatchlistDto>();

                try
                {
                    await dto.Matches.ParallelForEachAsync(async match =>
                    {
                        var detail = await GetMatchResultAsync(match.GameId);
                        results.Add(detail);
                    }, maxDegreeOfParallelism: 10);
                  
                    return results.ToList();
                }
                catch (FlurlHttpException e)
                {
                    if (e.Call.HttpStatus == ((System.Net.HttpStatusCode)429))
                    {
                        if (tooManyRequestsThrowException)
                        {
                            throw e;
                        }
                        else
                        {
                            return results.ToList();
                        }
                    }

                    // Propagate any other type of exception other than 429
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<MatchlistDto> GetTotalMatches(int seasonId, string queue)
        {
            MatchlistDto matchlist = null;
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

                    if (matchlist == null)
                    {
                        matchlist = current;
                    }
                    else
                    {
                        matchlist.Matches.AddRange(current.Matches);
                        matchlist.endIndex = current.endIndex;
                    }

                    request.SetQueryParam("beginIndex", (request.QueryParams["beginIndex"] as int?) + 100);

                } while (!completed);

                // Trim extra matches
                int removedMatches = matchlist.Matches.RemoveAll(m => m.Timestamp < seasonTimestamp);
                matchlist.endIndex -= removedMatches;
                matchlist.totalGames = matchlist.endIndex + 1;

                return matchlist;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("GetTotalMatches throwed an Exception: {0}", e.Message));
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
