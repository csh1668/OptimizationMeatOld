using System.Collections;
using RimWorld;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AlienMeatTest;
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

        public static readonly string VERSION = "1.2.3";
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

            MeatOptimization.PostOptimize();

            sp.Stop();

            MeatLogger.Message(
                $"<color=red>{count}</color> meat defs have been removed from the game. Elapsed Time: {sp.ElapsedMilliseconds}ms");
        }
    }
}

//[HarmonyPatch(typeof(ThingSetMakerUtility)), HarmonyPatch("GetAllowedThingDefs")]
//class Test
//{
//    static IEnumerable<ThingDef> Postfix(IEnumerable<ThingDef> values, ThingSetMakerParams parms)
//    {
//        MeatLogger.Debug("Hello");
//        foreach (var def in parms.filter.AllowedThingDefs)
//        {
//            MeatLogger.DebugEnumerate(def.defName);
//        }
//        MeatLogger.Debug();

//        foreach (var value in values)
//        {
//            yield return value;
//        }
//    }
//}