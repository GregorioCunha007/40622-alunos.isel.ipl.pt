using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotSimplify.Dto
{
    public class MatchlistDto
    {
        public List<MatchReferenceDto> Matches;
        public int totalGames;
        public int startIndex;
        public int endIndex;
    }
}
