using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotSimplify.Dto
{
    public class UserInfoDto
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public string SummonerId { get; set; }
        public string Name { get; set; }
        public string Puuid { get; set; }
        public int ProfileIconId { get; set; }
        public string SummonerLevel { get; set; }
    }
}
