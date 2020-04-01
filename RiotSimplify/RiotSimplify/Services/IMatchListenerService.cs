using RiotSimplify.Publishers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotSimplify.Services
{
    public interface IMatchListenerService
    {
        void SubscribeForMatches(int season, string queue, MatchPublisher.MatchesReceivedEventHandler subscriberMethod);
    }
}
