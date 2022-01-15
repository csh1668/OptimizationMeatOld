using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace AlienMeatTest.Compatibility
{
    public class Compatibility_MoreFilters : Compatibility
    {
        protected override string PackageID => "jaxe.morefilters";
        public override bool IsPreOptimization => false;
        public override void DoPatch()
        {
            if (!DetectMod()) return;
            MeatLogger.Debug("MoreFilters Detected!");

            var rottable = ThingCategoryDef.Named("FilterRottable");
            var degradable = ThingCategoryDef.Named("FilterDegradable");
            rottable.childThingDefs
                .RemoveAll(def => MeatOptimization.RemovedMeatDefs.Contains(def.defName));
            degradable.childThingDefs
                .RemoveAll(def => MeatOptimization.RemovedMeatDefs.Contains(def.defName));
            rottable.ResolveReferences();
            degradable.ResolveReferences();
            ThingCategoryDefOf.Root.ResolveReferences();
        }
    }
}
