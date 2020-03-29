using RiotSimplify.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RiotSimplify.Publishers
{
    public class MatchesEventArgs : EventArgs
    {
        public List<MatchResult> Matches { get; set; }
    }

    public class MatchPublisher : Clients.MatchClient
    {
        public delegate void MatchesReceivedEventHandler(object source, MatchesEventArgs eventArgs);
        public event MatchesReceivedEventHandler MatchesReceived;

        public int LastMatchIndex { get; set; }

        public int CooldownTime = 120000;
            
        protected virtual void OnMatchesReceived(List<MatchResult> matches)
        {
            MatchesReceived?.Invoke(this, new MatchesEventArgs { Matches = matches });
        }

        public async void Listen(int season, string queue)
        {
            List<MatchResult> matches = null;

            try
            {
                matches = await GetMatchesBySeasonAsync(season, queue, LastMatchIndex, LastMatchIndex + 100, false);

                if (!matches.Any())
                {
                    return;
                }

                if (matches.Last().Timestamp <= Utils.GetSeasonTimestamp(season))
                {
                    // No more results for the time being. Cancel the retry
                    OnMatchesReceived(matches);
                }
                else
                {                    
                    LastMatchIndex = LastMatchIndex + matches.Count;
                    OnMatchesReceived(matches);
                    // After cooldown, keep listening while we have results for this season
                    Thread.Sleep(CooldownTime);
                    Listen(season, queue);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
                       
        }
        
    }
}
