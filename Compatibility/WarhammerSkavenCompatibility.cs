using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AlienMeatTest.Compatibility
{
    public static class WarhammerSkavenCompatibility
    {
        public static string PackageID { get; private set; } = "kompadt.warhammer.skaven";
        private static MeatModSettings settings = LoadedModManager.GetMod<MeatMod>().GetSettings<MeatModSettings>();
        private static readonly string SkavenMeat = "Meat_Alien_Skaven";
        public static void DoPatchIfDetected()
        {
            if (!DetectMod()) return;
            MeatLogger.Message("Warhammer: Skaven Detected!");
            if (!settings.OptimizationAnimalMeat) return;

            var humanMeatDef = ThingDef.Named("Meat_Human");
            var racesThatDropSkavenMeat =
                DefDatabase<ThingDef>.AllDefs.Where(x => x.race?.meatDef?.defName == SkavenMeat).ToList();
            foreach (var i in racesThatDropSkavenMeat)
            {
                var defName = i.defName;
                MeatOptimization.WhiteList.Add(defName);
                i.race.meatDef = humanMeatDef;
            }
            MeatOptimization.RemovedDefs.Add(ThingDef.Named(SkavenMeat).defName);

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
