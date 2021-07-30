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
    public static class AnimalLogicCompatibility
    {
        public static string PackageID { get; private set; } = "oblitus.animalslogic";

        public static void DoWarnIfDetected()
        {
            if (Detect(out IEnumerable settings))
            {
                MeatLogger.Message("Animals Logic Detected!");
                foreach (var setting in settings)
                {
                    if (setting is FieldInfo b && b.Name == "tastes_like_chicken" &&
                        b.GetValue(null) as bool? == true)
                    {
                        MeatLogger.Error("You need to turn off 'Convert any generic meat into chicken meat upon butchering' from Animals Logic and restart the game. Otherwise, weird bug will occur!");
                    }
                }
            }
        }

        private static bool Detect(out IEnumerable options)
        {
            options = null;
            return DetectMod() && DetectOption(out options);
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

        private static bool DetectOption(out IEnumerable options)
        {
            options = null;
            Type settings = Type.GetType("AnimalsLogic.Settings, AnimalsLogic");
            if (settings == null)
            {
                return false;
            }

            options = settings.GetFields().Where(x => x.IsStatic);
            return true;
        }
    }

}
