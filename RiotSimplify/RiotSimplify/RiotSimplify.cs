using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotSimplify
{
    public class RiotSimplify
    {
        private String _apiKey;
        private String _apiUrl = "https://euw1.api.riotgames.com";

        public String SummonerName { get; set; } 

        public RiotSimplify (string apiKey, string summonerName = null)
        {
            _apiKey = apiKey;
            SummonerName = summonerName;
        }
    }
}
