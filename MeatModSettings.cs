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
        private bool optimizationAnimalMeat = true;
        private bool optimizationAlienMeat = false;
        private bool optimizationFishMeat = false;
        private bool debugMode = false;

        public bool OptimizationAnimalMeat => optimizationAnimalMeat;
        public bool OptimizationAlienMeat => optimizationAlienMeat;
        public bool OptimizationFishMeat => optimizationFishMeat;
        public bool DebugMode => debugMode;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref optimizationAnimalMeat, "OM_animal", true);
            Scribe_Values.Look(ref optimizationAlienMeat, "OM_alien", false);
            Scribe_Values.Look(ref optimizationFishMeat, "OM_fish", false);
            Scribe_Values.Look(ref debugMode, "OM_debugMode", false);
            base.ExposeData();
        }

        public void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();
            ls.Begin(inRect);
            ls.Label("OM_restart_required".Translate());
            ls.CheckboxLabeled("OM_animal_setting".Translate(), ref optimizationAnimalMeat);
            ls.CheckboxLabeled("OM_alien_setting".Translate(), ref optimizationAlienMeat);
            if (Compatibility.VFECompatibility.Detect())
            {
                ls.CheckboxLabeled("OM_fish_setting".Translate(), ref optimizationFishMeat);
            }
            ls.CheckboxLabeled("OM_debug_setting".Translate(), ref debugMode);
            ls.End();
        }
    }
}
