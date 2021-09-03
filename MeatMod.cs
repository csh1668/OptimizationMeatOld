using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AlienMeatTest.Patches;
using HarmonyLib;
using Verse;
using RimWorld;
using UnityEngine;

namespace AlienMeatTest
{
    public class MeatMod : Mod
    {
        internal MeatModSettings settings;
        public MeatMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<MeatModSettings>();
            MeatLogger.debugMode = settings.DebugMode;


            Harmony h = new Harmony("com.seohyeon.optimization.meat");
            h.PatchAll(Assembly.GetExecutingAssembly());

        } 

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            settings.DoWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "OM_Category".Translate();
        }
    }
}
