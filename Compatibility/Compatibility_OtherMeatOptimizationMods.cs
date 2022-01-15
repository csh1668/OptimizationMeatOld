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
    public class Compatibility_OtherMeatOptimizationMods : Compatibility
    {
        protected override string PackageID => string.Empty;
        public override bool IsPreOptimization => true;
        public override void DoPatch()
        {
            if (DetectMod())
            {
                MeatLogger.Message("Other Meat Optimization Mod(s) Detected!");
                MeatLogger.Error("Only one 'Meat Optimization' mod should be used. Otherwise, weird bug will occur!");
            }
        }

        public override bool DetectMod()
        {
            List<string> PackageIDs = new List<string>
            {
                "owlchemist.optimizationmeats",
                "brucethemoose.optimizationmeats",
                "nightkosh.optimization",
                "brat.optimizehumanmeats"
            };
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
