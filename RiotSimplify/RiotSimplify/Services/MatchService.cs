using RiotSimplify.Dto;
using RiotSimplify.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RiotSimplify.Clients.MatchClient;

namespace RiotSimplify.Services
{
    public interface MatchService
    { 
        Task<List<MatchResult>> GetMatchesFromSeason(int seasonId, string queue, int beginIndex = 0, int endIndex = 100, bool tooManyRequestsThrowException = true);

        Task<MatchlistDto> GetTotalMatches(int seasonId, string queue);

        Task<MatchResult> GetMatchDetails(int matchId, Dictionary<string, string> queryStringOptions = null);
    }
}
