using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using RimWorld;
using UnityEngine;

namespace AlienMeatTest
{
    public class MeatModSettings : ModSettings
    {
        public static bool OptimizationAnimalMeat = true;
        public static bool OptimizationAlienMeat = false;
        public static bool OptimizationFishMeat = false;
        public static bool DebugMode = false;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref OptimizationAnimalMeat, "OM_animal", true);
            Scribe_Values.Look(ref OptimizationAlienMeat, "OM_alien", false);
            Scribe_Values.Look(ref OptimizationFishMeat, "OM_fish", false);
            Scribe_Values.Look(ref DebugMode, "OM_debugMode", false);
            base.ExposeData();
        }

        public void DoWindowContents(Rect inRect)
        {
            MeatLogger.debugMode = DebugMode;
            Listing_Standard ls = new Listing_Standard();
            ls.Begin(inRect);
            ls.Label("OM_restart_required".Translate());
            ls.CheckboxLabeled("OM_animal_setting".Translate(), ref OptimizationAnimalMeat);
            ls.CheckboxLabeled("OM_alien_setting".Translate(), ref OptimizationAlienMeat);
            if (Compatibility.VFECompatibility.Detect())
            {
                ls.CheckboxLabeled("OM_fish_setting".Translate(), ref OptimizationFishMeat);
            }
            ls.CheckboxLabeled("OM_debug_setting".Translate(), ref DebugMode);


            if (DebugMode && ls.ButtonText("DEBUG_WriteAllRaceMeat"))
            {
                DebugLog_WriteAllRaceMeat();
            }

            if (DebugMode && ls.ButtonText("DEBUG_FindRacesDropNotOptimizedMeat"))
            {
                DebugLog_FindRacesDropNotOptimizedMeat();
            }

            ls.End();
        }

        private void DebugLog_WriteAllRaceMeat()
        {
            var races = DefDatabase<ThingDef>.AllDefs.Where(x => x.race != null && x.race.IsMechanoid);
            MeatLogger.Debug(($"{"RaceDefName",15}|{"MeatDefName",15}|{"UseMeatFrom",15}").Replace(' ', '　'));
            foreach (var thingDef in races)
            {
                MeatLogger.Debug(
                    ($"{thingDef.defName,15}|{thingDef.race.meatDef?.defName ?? "null",15}|{thingDef.race.useMeatFrom?.defName ?? "null",15}").Replace(' ', '　'));
            }
        }

        private void DebugLog_FindRacesDropNotOptimizedMeat()
        {
            var races = DefDatabase<ThingDef>.AllDefs.Where(x => (x.race != null && !x.race.IsMechanoid));
            foreach (var thingDef in races)
            {
                if(thingDef.race.meatDef?.defName != "Meat_Human" &&
                   thingDef.race.meatDef?.defName != "Meat_Cow" &&
                   thingDef.race.meatDef?.defName != "Meat_Megaspider")
                    MeatLogger.Debug($"{thingDef.defName} drops not optimized meat {thingDef.race.meatDef?.defName ?? "null"}");
            }
            MeatLogger.Debug("If you only see this message, it means that there is no meat that is not optimized.");
        }
    }
}
