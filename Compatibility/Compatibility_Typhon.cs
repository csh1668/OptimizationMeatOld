using System.Linq;
using Verse;

namespace AlienMeatTest.Compatibility
{
    public class Compatibility_Typhon : Compatibility
    {
        protected override string PackageID => "caaz.typhon";
        public override bool IsPreOptimization => true;
        public override void DoPatch()
        {
            if (!DetectMod()) return;
            MeatLogger.Debug("Typhon Detected!");
            string typhonOrgan = "TyphonOrgan";
            var typhonRaces = DefDatabase<ThingDef>.AllDefs.Where(x => x.race?.meatDef?.defName == typhonOrgan).ToList();
            foreach (var i in typhonRaces)
            {
                MeatOptimization.WhiteListRace.Add(i.defName);
            }
            MeatOptimization.WhiteListMeat.Add("TyphonOrgan");
        }
    }
}
