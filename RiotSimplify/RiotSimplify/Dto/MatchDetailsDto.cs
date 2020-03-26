using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotSimplify.Dto
{
    public class MatchDetailsDto
    {
        public long GameId { get; set; }

        public List<ParticipantDetails> Participants { get; set; }

        public List<ParticipantIdentities> ParticipantIdentities { get; set; }
    }

    public class ParticipantIdentities
    {
        public int ParticipantId { get; set; }
        public Player Player { get; set; }
    }

    public class ParticipantDetails
    {
        public int ParticipantId { get; set; }
        public int ChampionId { get; set; }
        public Stat Stats { get; set; }
    }

    public class Stat
    {
        public bool Win { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public int GoldEarned { get; set; }
    }

    public class Player
    {
        public string AccountId { get; set; }
    }
}
