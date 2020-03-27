using Flurl;
using Flurl.Http;
using RiotSimplify.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotSimplify.Clients
{
    public class UserClient
    {
        public string summonerEndpoint = "/lol/summoner/v4/summoners/by-name/";
        private UserInfoDto currentUser { get; set; }
        public async Task<string> GetAccountId(string summonerName)
        {
            if (currentUser != null)
            {
                return currentUser.AccountId;
            }

            try
            {
                Url url = RiotApiUtils.Api
                    .AppendPathSegment(summonerEndpoint)
                    .AppendPathSegment(summonerName)
                    .SetQueryParam("api_key", RiotApiUtils.ApiKey);

                currentUser = await url.GetJsonAsync<UserInfoDto>();

                return currentUser.AccountId;
            } catch(FlurlHttpException e)
            {
                throw new Exception(string.Format("Error getting info for summoner name {0} | Status code: {1}", summonerName, e.Call.HttpStatus), e);
            }
        }

        public async Task<string> GetSummonerIconPath(string summonerName)
        {
            if (currentUser != null)
            {
                return Utils.GetSummonerIconPath(currentUser.ProfileIconId);
            }

            try
            {
                Url url = RiotApiUtils.Api
                    .AppendPathSegment(summonerEndpoint)
                    .AppendPathSegment(summonerName)
                    .SetQueryParam("api_key", RiotApiUtils.ApiKey);

                currentUser = await url.GetJsonAsync<UserInfoDto>();

                return Utils.GetSummonerIconPath(currentUser.ProfileIconId);
            }
            catch (FlurlHttpException e)
            {
                throw new Exception(string.Format("Error getting info for summoner name {0} | Status code: {1}", summonerName, e.Call.HttpStatus), e);
            }
        }
    }
}
