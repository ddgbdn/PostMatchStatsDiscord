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
            = @"D:\StratzPostMatch\Data\ObtainedMatches.json";

        public static string NotParsedMatchesPath
            = @"D:\StratzPostMatch\Data\NotParsedMatches.json";

        public static string ButtPlug
            = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\") + "Buttplug.jpg");
    }
}
