using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using HarmonyLib;
using Verse;
using RimWorld;
using AlienMeatTest.Compatibility;
using AlienMeatTest.Patches;

namespace AlienMeatTest
{
    [StaticConstructorOnStartup]
    public static class SeoHyeon
    {
        public static readonly string MOD_NAME = "Optimization: Meats - C# Edition";
        public static readonly string MOD_NAME_COLORED = $"<color=blue>{MOD_NAME}</color>";
#if RimWorld12
        public static readonly string GAME_VERSION = "1.2";
#else
        public static readonly string GAME_VERSION = "1.3";
#endif

        public static readonly string VERSION = "1.2.4d";

        /*
         * TODO: 현재 구상중인 새로운 알고리즘
         * ThingDefGenerator을 Harmony를 통해 고기 생성 메서드를 갈아치운다
         * 고기를 생성할 때 쇠고기로 통합
         * DefDatabase<ThingDef>.GetNamed을 Prefix로 Meat_로 시작할 경우 통합된 고기(쇠고기)를 돌려주기
         */


        static SeoHyeon()
        {
            //Harmony h = new Harmony("com.seohyeon.optimization.meat");
            //h.PatchAll(Assembly.GetExecutingAssembly());
            //if (!DefGeneratorPatch.PatchExecuted)
            //{
            //    MeatLogger.Error("The patch is not executed, report this error to developer");
            //}

            MeatLogger.Message("Mod version: " + VERSION + " for RimWorld version: " + GAME_VERSION);

            MeatLogger.Debug("Debug Mode!");
            //Stopwatch sp = new Stopwatch();
            //sp.Start();

            //AnimalsLogicCompatibility.DoWarnIfDetected();
            //OtherOMCompatibility.DoWarnIfDetected();

            //int count = 0,
            //    meatCount = MeatOptimization.OptimizeMeat(),
            //    fishCount = VFECompatibility.OptimizeFishIfDetected();

            //count += meatCount + fishCount;

            //MeatPostOptimization.PostOptimize();


            //sp.Stop();

            //MeatLogger.Message(
            //    $"<color=red>{count}</color> meat defs have been removed from the game. Elapsed Time: {Math.Round(sp.Elapsed.TotalSeconds,2)}sec");
        }
    }
}

//[HarmonyPatch(typeof(ThingSetMakerUtility)), HarmonyPatch("GetAllowedThingDefs")]
//class Test
//{
//    static IEnumerable<ThingDef> Postfix(IEnumerable<ThingDef> values, ThingSetMakerParams parms)
//    {
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