using System.Collections;
using RimWorld;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Verse;
using AlienMeatTest.Compatibility;

namespace AlienMeatTest
{
    [StaticConstructorOnStartup]
    public static class SeoHyeon
    {
#if RimWorld12
        public static readonly string GAME_VERSION = "1.2";
#else
        public static readonly string GAME_VERSION = "1.3";
#endif

        public static readonly string VERSION = "1.2.2";
        public static readonly string MOD_NAME = "Optimization: Meats - C# Edition";
        public static readonly string MOD_NAME_COLORED = $"<color=blue>{MOD_NAME}</color>";

        static SeoHyeon()
        {
            //Harmony h = new Harmony("com.seohyeon.optimization.meat");
            //h.PatchAll(Assembly.GetExecutingAssembly());

            MeatLogger.Message("Mod version: " + VERSION + " for RimWorld version: " + GAME_VERSION);

            MeatLogger.Debug("Debug Mode!");
            Stopwatch sp = new Stopwatch();
            sp.Start();

            AnimalsLogicCompatibility.DoWarnIfDetected();
            OtherOMCompatibility.DoWarnIfDetected();

            int count = 0,
                meatCount = MeatOptimization.OptimizeMeat(),
                fishCount = VFECompatibility.OptimizeFishIfDetected();

            count += meatCount + fishCount;

            sp.Stop();

            MeatLogger.Message(
                $"<color=red>{count}</color> meat defs have been removed from the game. Elapsed Time: {sp.ElapsedMilliseconds}ms");
        }
    }
}

//    [HarmonyPatch(typeof(DrugPolicyDatabase)), HarmonyPatch("NewDrugPolicyFromDef")]
//    class Test
//    {
//        static bool Prefix(DrugPolicyDatabase __instance, DrugPolicyDef def, ref DrugPolicy __result)
//        {
//            __result = __instance.MakeNewDrugPolicy();
//            __result.label = def.LabelCap;
//            __result.sourceDef = def;
//            if (def.allowPleasureDrugs)
//            {
//                for (int i = 0; i < __result.Count; i++)
//                {
//                    if (__result[i].drug.IsPleasureDrug)
//                    {
//                        __result[i].allowedForJoy = true;
//                    }
//                }
//            }
//            if (def.entries != null)
//            {
//                for (int j = 0; j < def.entries.Count; j++)
//                {
//                    MeatLogger.Debug($"{def.entries[j].drug.defName}");
//                    __result[def.entries[j].drug].CopyFrom(def.entries[j]);
//                }
//            }

//            return false;
//        }
//    }
//}