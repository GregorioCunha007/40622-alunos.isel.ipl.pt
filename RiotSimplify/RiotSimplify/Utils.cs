using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RiotSimplify
{
    public static class Utils
    {
        private static string LoadJson(string path)
        {
            try
            {
                using (StreamReader r = new StreamReader(path))
                {
                    if (r == null)
                    {
                        throw new Exception("File has no content");
                    }

                    string json = r.ReadToEnd();
                    return json;
                }

            } catch(Exception e)
            {
                throw new Exception(string.Format("Error loading from file path {0}", path), e);
            }
        }
        
        public static long GetSeasonTimestamp(int seasonId)
        {
            try
            {
                
                dynamic json = JsonConvert.DeserializeObject(LoadJson(@"D:\Riot-Simplify\RiotSimplify\RiotSimplify\StaticData\patches.json"));

                foreach (var patch in json.patches)
                {
                    string name = patch.name;

                    // Start of season X.1
                    if (name == seasonId + ".1")
                    {
                        return patch.start;
                    }
                }

                return 0;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Failed getting timestamp for seasonId {0}", seasonId), e);
            }
        }

        public static string GetSummonerIconPath(int summonerIcon)
        {
            try
            {

                dynamic icons = JsonConvert.DeserializeObject(LoadJson(@"D:\Riot-Simplify\RiotSimplify\RiotSimplify\StaticData\summoner-icons.json"));

                foreach (var icon in icons)
                {
                    if (icon.id == summonerIcon)
                    {
                        return icon.imagePath;
                    }
                }

                throw new Exception(string.Format("No Summonner icon with ID: {0}", summonerIcon), new ArgumentException());
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Failed getting summoner icon for id {0}", summonerIcon), e);
            }
        }

        public static Url AppendQueryString(this Url url, Dictionary<string, string> queryString)
        {
            if (queryString == null)
            {
                return url;
            }

            foreach(KeyValuePair<string, string> keyVal in queryString)
            {
                url.SetQueryParam(keyVal.Key, keyVal.Value);
            }

            return url;
        }
    }
}
