﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;

namespace AlienMeatTest.Compatibility
{
    // TODO: make VFECompatibility 'Object-Oriented'.
    public static class VFECompatibility
    {
        public static string PackageID { get; private set; } = "vanillaexpanded.vcef";
        public static List<string> RemovedDefs { get; private set; } = new List<string>();
        private static MeatModSettings settings = LoadedModManager.GetMod<MeatMod>().GetSettings<MeatModSettings>();

        private static Type fishDefTypeOf;
        private static Type fishDefDatabaseTypeOf;

        public static int OptimizeFishIfDetected()
        {
            if (!Detect()) return 0;
            MeatLogger.Message("Vanilla Fishing Expanded Detected!");
            if (!settings.OptimizationFishMeat) return 0;
            MeatLogger.Debug("Fish Optimizing...");
            fishDefTypeOf = Type.GetType("VCE_Fishing.FishDef, VCE-Fishing");
            fishDefDatabaseTypeOf = typeof(DefDatabase<>).MakeGenericType(fishDefTypeOf);
            int result = OptimizeFish();
            MeatLogger.Debug("Fish Optimized!");
            return result;
        }

        public static int OptimizeFish()
        {
            if (fishDefTypeOf == null)
            {
                MeatLogger.Error("Can't find FishDef from VCE_Fishing");
                return 0;
            }

            IEnumerable fishDefs = fishDefDatabaseTypeOf.GetProperty("AllDefs")?.GetValue(null) as IEnumerable;
            var toRemoveFishDefs = new List<string>();
            var toRemoveThingDefs = new List<string>();

            Def smallFish = null, mediumFish = null, largeFish = null;

            foreach (var fish in fishDefs)
            {
                if (fish == null) continue;
                var name = fishDefTypeOf.GetField("defName").GetValue(fish) as string;
                MeatLogger.DebugEnumerate(name);
                if (name == "VCEF_AnchovyFish")
                    smallFish = fish as Def;
                else if (name == "VCEF_MackerelFish")
                    mediumFish = fish as Def;
                else if (name == "VCEF_SalmonFish")
                    largeFish = fish as Def;
            }
            MeatLogger.DebugEnumerate(fishDefs.EnumerableCount().ToString());
            MeatLogger.Debug();

            if (smallFish == null || mediumFish == null || largeFish == null)
            {
                MeatLogger.Error("Can't find required fishes");
                return 0;
            }

            fishDefTypeOf.GetField("canBeFreshwater").SetValue(smallFish, true);
            fishDefTypeOf.GetField("canBeFreshwater").SetValue(mediumFish, true);
            fishDefTypeOf.GetField("canBeFreshwater").SetValue(largeFish, true);
            fishDefTypeOf.GetField("canBeSaltwater").SetValue(smallFish, true);
            fishDefTypeOf.GetField("canBeSaltwater").SetValue(mediumFish, true);
            fishDefTypeOf.GetField("canBeSaltwater").SetValue(largeFish, true);

            ((ThingDef) fishDefTypeOf.GetField("thingDef").GetValue(smallFish)).label =
                "OM_smallFishLabel".Translate();
            ((ThingDef)fishDefTypeOf.GetField("thingDef").GetValue(mediumFish)).label =
                "OM_mediumFishLabel".Translate();
            ((ThingDef)fishDefTypeOf.GetField("thingDef").GetValue(largeFish)).label =
                "OM_largeFishLabel".Translate();

            List<string> biomes = new List<string>();


            var terranTypeOf = Type.GetType("VCE_Fishing.BiomeTempDef, VCE-Fishing");
            var terran = typeof(DefDatabase<>).MakeGenericType(terranTypeOf).GetProperty("AllDefs")?
                .GetValue(null) as IEnumerable;

            foreach (var i in terran)
            {
                var label = (string) (i.GetType().GetField("biomeTempLabel").GetValue(i));
                biomes.Add(label);
                MeatLogger.DebugEnumerate(label);
            }

            MeatLogger.Debug();

            fishDefTypeOf.GetField("allowedBiomes").SetValue(smallFish, biomes);
            fishDefTypeOf.GetField("allowedBiomes").SetValue(mediumFish, biomes);
            fishDefTypeOf.GetField("allowedBiomes").SetValue(largeFish, biomes);

            foreach (var fish in fishDefs)
            {
                if (fish == null) continue;

                var defName = fishDefTypeOf.GetField("defName").GetValue(fish) as string;
                if (defName == "VCEF_AnchovyFish" || defName == "VCEF_MackerelFish" ||
                    defName == "VCEF_SalmonFish" || defName == "VCEF_PufferfishFish")
                    continue;
                // defName == "VCEF_PufferfishFish"
                // 복어는 특별한 hediff을 가지고 있으니까 추가할까?
                var fishSize = fishDefTypeOf.GetField("fishSizeCategory").GetValue(fish);
                if (fishSize.ToString() == "Special")
                    continue;

                var thingDefName = (fishDefTypeOf.GetField("thingDef").GetValue(fish) as ThingDef)?.defName;

                toRemoveFishDefs.Add(defName);
                toRemoveThingDefs.Add(thingDefName);
            }

            toRemoveFishDefs = toRemoveFishDefs.Distinct().ToList();
            toRemoveThingDefs = toRemoveThingDefs.Distinct().ToList();

            RemovedDefs.Clear();
            RemovedDefs.AddRange(toRemoveThingDefs);

            RemoveDefs(toRemoveFishDefs, toRemoveThingDefs);

            return toRemoveFishDefs.Count;
        }

        private static void RemoveDefs(List<string> fishDefs, List<string> fishThingDefs)
        {
            if (fishDefs.Count != fishThingDefs.Count)
            {
                MeatLogger.Error("The counts of the two lists are different.");
                return;
            }

            foreach (var categoryDef in DefDatabase<ThingCategoryDef>.AllDefs.SelectMany(x => x.ThisAndChildCategoryDefs))
            {
                foreach (var thingDef in fishThingDefs)
                {
                    categoryDef.childThingDefs.Remove(ThingDef.Named(thingDef));
                    //MeatLogger.Debug($"{thingDef} is removed from {categoryDef.defName}");
                }
            }

            foreach (var categoryDef in DefDatabase<ThingCategoryDef>.AllDefs.SelectMany(x => x.ThisAndChildCategoryDefs))
            {
                categoryDef.ResolveReferences();
            }

            var VCEF_RawFishCategory = ThingCategoryDef.Named("VCEF_RawFishCategory");
            MethodInfo removeFishMethod = fishDefDatabaseTypeOf.GetMethod("Remove", BindingFlags.Static | BindingFlags.NonPublic);
            MethodInfo removeThingMethod = typeof(DefDatabase<ThingDef>).GetMethod("Remove", BindingFlags.Static | BindingFlags.NonPublic);
            MethodInfo getNamedMethod = fishDefDatabaseTypeOf.GetMethod("GetNamed", BindingFlags.Static | BindingFlags.Public);
            
            for (int i = 0; i < fishDefs.Count; i++)
            {
                object fishDef = getNamedMethod?.Invoke(null, new object[] {fishDefs[i], true});
                ThingDef thingDef = ThingDef.Named(fishThingDefs[i]);

                VCEF_RawFishCategory.childThingDefs.Remove(thingDef);
                removeFishMethod.Invoke(null, new object[] {fishDef});
                removeThingMethod.Invoke(null, new object[] {thingDef});
            }

            

            VCEF_RawFishCategory.ResolveReferences();
            ThingCategoryDefOf.MeatRaw.ResolveReferences();
            ThingCategoryDefOf.Foods.ResolveReferences();
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
