using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostMatchStatsDiscord.Constants
{
    public static class Paths
    {
        public static string ObtainedMathesPath 
            = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\") + "ObtainedMatches.json");

        public static string NotParsedMatchesPath
            = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\") + "NotParsedMatches.json");
    }
}
