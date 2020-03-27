using RiotSimplify.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotSimplify.Services
{
    public interface MatchService
    {
        Task<List<MatchResult>> GetMatchesFromSeason(int seasonId, string queue);

        Task<MatchResult> GetMatchDetails(int matchId, Dictionary<string, string> queryStringOptions = null);
    }
}
