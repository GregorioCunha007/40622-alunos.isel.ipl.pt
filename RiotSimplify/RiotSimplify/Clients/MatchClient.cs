using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSimplify.Mappers;
using Flurl.Http;

namespace RiotSimplify.Clients
{
    public class MatchClient
    {

        private String _matchListEndpoint = "/lol/match/v4/matchlists/by-account/";

        public String AccountId { get; set; }

        internal List<MatchResult> GetMatchesBySeason(int seasonId, Dictionary<string, string> queryStringOptions)
        {
            if (String.IsNullOrEmpty(AccountId))
            {
                throw new Exception("AccountId cannot be null or empty");
            }

            long seasonTimestamp = Utils.GetSeasonTimestamp(seasonId); 

            throw new NotImplementedException();
        }
    }
}
