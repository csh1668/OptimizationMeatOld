using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AlienMeatTest.Compatibility
{
    public static class OtherOMCompatibility
    {
        public static List<string> PackageIDs { get; private set; } = new List<string>
        {
            "owlchemist.optimizationmeats", 
            "brucethemoose.optimizationmeats",
            "nightkosh.optimization",
            "brat.optimizehumanmeats"
        };

        public static void DoWarnIfDetected()
        {
            if (Detect())
            {
                MeatLogger.Message("Other Meat Optimization Mod(s) Detected!");
                MeatLogger.Error("Only one should be used. Otherwise, weird bug will occur!");
            }
        }

        public static bool Detect()
        {
            return DetectMod();
        }
        private static bool DetectMod()
        {
            foreach (var mods in ModLister.AllInstalledMods.Where(x => x.Active))
            {
                if (PackageIDs.Contains(mods.PackageId))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
