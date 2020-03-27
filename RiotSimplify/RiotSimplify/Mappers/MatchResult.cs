using RiotSimplify.Dto;
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
        public bool Win { get; set; }
        public int ChampionId { get; set; }
        public double TotalGold { get; set; }       
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public long Timestamp { get; set; }

        public static MatchResult MapFrom(MatchDetailsDto matchDetailsDto, string Key)
        {
            var participantId = matchDetailsDto.ParticipantIdentities.First(p => p.Player.AccountId == Key).ParticipantId;
            var playerMatchInfo = matchDetailsDto.Participants.First(p => p.ParticipantId == participantId);
            bool won = playerMatchInfo.Stats.Win;
            int championId = playerMatchInfo.ChampionId;

            return new MatchResult
            {
                Win = playerMatchInfo.Stats.Win,
                Position = RiotApiUtils.GetPosition(playerMatchInfo.Timeline.Lane, playerMatchInfo.Timeline.Role),
                TotalGold = playerMatchInfo.Stats.GoldEarned,
                Kills = playerMatchInfo.Stats.Kills,
                Assists = playerMatchInfo.Stats.Assists,
                Deaths = playerMatchInfo.Stats.Deaths,
                ChampionId = playerMatchInfo.ChampionId,
                Timestamp = matchDetailsDto.GameCreation
            };
        }
    }
}
