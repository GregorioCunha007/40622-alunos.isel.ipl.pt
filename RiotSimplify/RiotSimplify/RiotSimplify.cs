using RiotSimplify.Clients;
using RiotSimplify.Mappers;
using RiotSimplify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotSimplify
{
    public class RiotSimplify : MatchService, UserAccountService
    {
        private MatchClient matchClient;
        private UserClient userClient;

        public String SummonerName { get; set; } 

        public RiotSimplify (string apiKey, string summonerName = null)
        {
            RiotApiUtils.ApiKey = apiKey;
            SummonerName = summonerName;
            matchClient = new MatchClient();
            userClient = new UserClient();
        }

        public async Task<List<MatchResult>> GetMatchesFromSeason(int seasonId, string queue)
        {
            try
            {
                if (string.IsNullOrEmpty(RiotApiUtils.AccountId))
                {
                    RiotApiUtils.AccountId = await userClient.GetAccountId(SummonerName);
                }

                return await matchClient.GetMatchesBySeasonAsync(seasonId, queue);
            } catch(Exception e)
            {
                throw new Exception(string.Format("Failed to get matches for season {0}", seasonId), e);
            }
        }

        public async Task<MatchResult> GetMatchDetails(int matchId, Dictionary<string, string> queryStringOptions = null)
        {
            try
            {               
                return await matchClient.GetMatchResultAsync(matchId, queryStringOptions);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Failed to get match with id {0}", matchId), e);
            }
        }

        public async Task<string> GetSummonerIcon()
        {
            try
            {
                return await userClient.GetSummonerIconPath(SummonerName);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get summonner icon", e);
            }
        }

        public void SetSummonerName(string newSummonerName)
        {
            SummonerName = newSummonerName;
        }
    }
}
