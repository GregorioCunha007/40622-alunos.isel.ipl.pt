﻿using System;
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

        public static string AccountId { get; internal set; }

        public static string GetPosition(string lane, string role)
        {
            if (lane == "MIDDLE" || lane == "MID") return "MIDDLE";
            if (lane == "TOP") return "TOP";
            if (lane == "JUNGLE") return "JUNGLE";
            if ((lane == "BOTTOM" || (lane == "NONE" && role == "DUO_CARRY"))) return "BOTTOM";
            if ((lane == "BOTTOM" || (lane == "NONE" && role == "DUO_SUPPORT"))) return "SUPPORT";
            else return "UNKNOWN";
        }
        
    }
}
