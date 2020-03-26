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
        List<MatchResult> GetMatchesFromSeason(int seasonId, Dictionary<string, string> queryStringOptions = null);
    }
}
