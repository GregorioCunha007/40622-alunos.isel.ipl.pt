using RiotSimplify.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotSimplify
{
    public static class RiotApiUtils
    {
        public static string Api = "https://euw1.api.riotgames.com";
        public static string IconsApi = "https://ddragon.leagueoflegends.com/cdn/6.3.1/img/profileicon/";
        public static string ApiKey { get; internal set; }
        public static string AccountId { get; internal set; }
        public static string SummonerName { get; internal set; }
        private static UserClient UserClient = new UserClient();

        public static string GetPosition(string lane, string role)
        {
            if (lane == "MIDDLE" || lane == "MID") return "MIDDLE";
            if (lane == "TOP") return "TOP";
            if (lane == "JUNGLE") return "JUNGLE";
            if ((lane == "BOTTOM" || (lane == "NONE" && role == "DUO_CARRY"))) return "BOTTOM";
            if ((lane == "BOTTOM" || (lane == "NONE" && role == "DUO_SUPPORT"))) return "SUPPORT";
            else return "UNKNOWN";
        }

        public static int GetQueueID(string queue)
        {
            if (queue == "SOLODUO") return 420;
            else return 0;
        }
        
        public static async Task FetchSummonerData ()
        {
            if (string.IsNullOrEmpty(AccountId))
            {
                var accId = await UserClient.GetAccountId(SummonerName);
                AccountId = accId;
            }
        }
    }
}
