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
    public class RiotSimplify : MatchService
    {
        private String _apiKey;
        private String _apiUrl = "https://euw1.api.riotgames.com";
        private MatchClient _matchClient;

        public String SummonerName { get; set; } 

        public RiotSimplify (string apiKey, string summonerName = null)
        {
            _apiKey = apiKey;
            SummonerName = summonerName;
            _matchClient = new MatchClient();
        }

        public List<MatchResult> GetMatchesFromSeason(int seasonId, Dictionary<string, string> queryStringOptions = null)
        {
            try
            {
                return _matchClient.GetMatchesBySeason(seasonId, queryStringOptions);

            } catch(Exception e)
            {
                throw new Exception(string.Format("Failed to get matches for season {0}", seasonId), e);
            }
        }
    }
}
