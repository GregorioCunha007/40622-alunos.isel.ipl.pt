using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotSimplify
{
    public static class RiotApiUtils
    {
        public static string Api = "https://euw1.api.riotgames.com";

        public static string ApiKey { get; internal set; }

        public static string GetPosition(string lane, string role)
        {
            if (lane == "MID_LANE" && role == "SOLO") return "MIDDLE";
            if (lane == "TOP_LANE" && role == "SOLO") return "TOP";
            if (lane == "JUNGLE"   && role == "NONE") return "JUNGLE";
            if (lane == "BOT_LANE" && role == "DUO_CARRY") return "BOTTOM";
            if (lane == "BOT_LANE" && role == "DUO_SUPPORT") return "SUPPORT";
            else return "UNKNOWN";
        }
    }
}
