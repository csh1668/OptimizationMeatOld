﻿using System;
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

        public static readonly string VERSION = "1.2.4h";

        internal static int Count;
        

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
            Stopwatch sp = new Stopwatch();
            sp.Start();

            // PreOptimization
            CompatibilityDatabase.InitDatabase();
            foreach (var patch in CompatibilityDatabase.All.Where(x => x.IsPreOptimization))
            {
                try
                {
                    patch.DoPatch();
                }
                catch (Exception e)
                {
                    MeatLogger.Error($"Error in {patch}: {e.Message}");
                }
            }

            // Optimization
            int meatCount = MeatOptimization.OptimizeMeat();
            Count += meatCount;

            //PostOptimization
            foreach (var patch in CompatibilityDatabase.All.Where(x => !x.IsPreOptimization))
            {
                try
                {
                    patch.DoPatch();
                }
                catch (Exception e)
                {
                    MeatLogger.Error($"Error in {patch}: {e.Message}");
                }
            }
            int fishCount = VFECompatibility.OptimizeFishIfDetected();
            Count += fishCount;
            MeatPostOptimization.PostOptimize();


            sp.Stop();

            MeatLogger.Message(
                $"<color=red>{Count}</color> meat defs have been removed from the game. Elapsed Time: {Math.Round(sp.Elapsed.TotalSeconds,2)}sec");
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