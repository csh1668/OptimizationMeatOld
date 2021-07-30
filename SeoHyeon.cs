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

        public static readonly string VERSION = "1.2.1";
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

            AnimalLogicCompatibility.DoWarnIfDetected();
            OtherOMCompatibility.DoWarnIfDetected();

            int count = 0,
                meatCount = MeatOptimization.OptimizeMeat(),
                fishCount = VFECompatibility.OptimizeFishIfDetected();

            count += meatCount + fishCount;
            MeatLogger.Debug($"optimized meats: {meatCount}, optimized fishes: {fishCount}");

            sp.Stop();

            MeatLogger.Message($"<color=red>{count}</color> meat defs have been removed from the game. Elapsed Time: {sp.ElapsedMilliseconds}ms");
        }
    }

    //[HarmonyPatch(typeof(ThingDefGenerator_Meat)), HarmonyPatch("ImpliedMeatDefs")]
    //class Test
    //{
    //    static IEnumerable<ThingDef> Postfix(IEnumerable<ThingDef> values)
    //    {
    //        MeatLogger.Message("Hello");
    //        yield return MeatOptimization.MakeNewRawMeat();
    //        foreach (var value in values)
    //        {
    //            yield return value;
    //        }
    //    }
    //}
}