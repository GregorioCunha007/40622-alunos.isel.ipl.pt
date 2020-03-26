using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotSimplify.Mappers
{
    public class MatchResult
    {
        public string Position { get; set; }
        public Score Result { get; set; }
        public int ChampionId { get; set; }
        public double TotalGold { get; set; }       
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public long Timestamp { get; set; }
    }

    public enum Score
    {
        LOSS, WIN
    }
}
