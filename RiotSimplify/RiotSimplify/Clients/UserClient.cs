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
        public string summonerEntries = "/lol/league/v4/entries/by-summoner/";
        private UserInfoDto currentUser { get; set; }
        public async Task<string> GetAccountId(string summonerName)
        {
            if (currentUser != null)
            {
                return currentUser.AccountId;
            }

            try
            {
                currentUser = await GetUserInfo();

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
                    .AppendPathSegment(currentUser.SummonerId)
                    .SetQueryParam("api_key", RiotApiUtils.ApiKey);

                currentUser = await url.GetJsonAsync<UserInfoDto>();

                return RiotApiUtils.IconsApi + currentUser.ProfileIconId + ".png";
            }
            catch (FlurlHttpException e)
            {
                throw new Exception(string.Format("Error getting info for summoner name {0} | Status code: {1}", summonerName, e.Call.HttpStatus), e);
            }
        }

        public async Task<List<EntryDto>> GetEntries()
        {            
            try
            {
                if (currentUser == null)
                {
                    currentUser = await GetUserInfo();
                }

                Url url = RiotApiUtils.Api
                    .AppendPathSegment(summonerEntries)
                    .AppendPathSegment(currentUser.Id)
                    .SetQueryParam("api_key", RiotApiUtils.ApiKey);

                return await url.GetJsonAsync<List<EntryDto>>();
            }
            catch (FlurlHttpException e)
            {
                throw new Exception(string.Format("Error getting info for summoner entries {0} | Status code: {1}", RiotApiUtils.SummonerName, e.Call.HttpStatus), e);
            }
        }

        private async Task<UserInfoDto> GetUserInfo()
        {
            try
            {
                Url url = RiotApiUtils.Api
                     .AppendPathSegment(summonerEndpoint)
                     .AppendPathSegment(RiotApiUtils.SummonerName)
                     .SetQueryParam("api_key", RiotApiUtils.ApiKey);

                currentUser = await url.GetJsonAsync<UserInfoDto>();

                return currentUser;
            }
            catch (FlurlHttpException e)
            {
                throw new Exception(string.Format("Error getting info for summoner name {0} | Status code: {1}", RiotApiUtils.SummonerName, e.Call.HttpStatus), e);
            }
        }
    }
}
