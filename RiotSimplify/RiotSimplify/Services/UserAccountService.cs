using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotSimplify.Services
{
    public interface UserAccountService
    {
        Task<string> GetSummonerIcon();
    }
}
