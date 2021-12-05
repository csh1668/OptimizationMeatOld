using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace AlienMeatTest.Compatibility
{
    public static class MoreFiltersCompatibility
    {
        public static string PackageID { get; private set; } = "jaxe.morefilters";

        public static void DoPatchIfDetected()
        {
            if (!Detect()) return;
            MeatLogger.Debug("MoreFilters Found!");

            var rottable = ThingCategoryDef.Named("FilterRottable");
            var degradable = ThingCategoryDef.Named("FilterDegradable");
            rottable.childThingDefs
                .RemoveAll(def => MeatOptimization.RemovedDefs.Contains(def.defName));
            degradable.childThingDefs
                .RemoveAll(def => MeatOptimization.RemovedDefs.Contains(def.defName));
            rottable.ResolveReferences();
            degradable.ResolveReferences();
            ThingCategoryDefOf.Root.ResolveReferences();
        }

        public static bool Detect()
        {
            return DetectMod();
        }

        private static bool DetectMod()
        {
            foreach (var mods in ModLister.AllInstalledMods.Where(x => x.Active))
            {
                if (mods.PackageId == PackageID)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
