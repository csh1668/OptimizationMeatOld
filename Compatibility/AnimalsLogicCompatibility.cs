using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Verse;
using RimWorld;

namespace AlienMeatTest.Compatibility
{
    public static class AnimalsLogicCompatibility
    {
        public static string PackageID { get; private set; } = "oblitus.animalslogic";
        private static IEnumerable animalsLogicSettings = null;

        public static void DoWarnIfDetected()
        {
            if (Detect())
            {
                DoWarn();
            }
        }

        private static void DoWarn()
        {
            MeatLogger.Message("Animals Logic Detected!");
            foreach (var setting in animalsLogicSettings)
            {
                if (setting is FieldInfo b && b.Name == "tastes_like_chicken" &&
                    b.GetValue(null) as bool? == true)
                {
                    MeatLogger.Error("You need to turn off 'Convert any generic meat into chicken meat upon butchering' from Animals Logic and restart the game. Otherwise, weird bug will occur!");
                }
            }
        }

        public static bool Detect()
        {
            if (DetectMod())
            {
                if (DetectOption())
                    return true;
                MeatLogger.Warn("Animals Logic is detected, but can't find its mod settings");
            }

            return false;
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

        private static bool DetectOption()
        {
            Type settings = Type.GetType("AnimalsLogic.Settings, AnimalsLogic");
            if (settings == null)
            {
                return false;
            }

            AnimalsLogicCompatibility.animalsLogicSettings = settings.GetFields().Where(x => x.IsStatic);
            return true;
        }
    }

}
