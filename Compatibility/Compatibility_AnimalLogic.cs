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
    public class Compatibility_AnimalLogic : Compatibility
    {
        protected override string PackageID => "oblitus.animalslogic";
        public override bool IsPreOptimization => true;

        public override void DoPatch()
        {
            if (DetectMod())
            {
                MeatLogger.Error("You need to turn off 'Convert any generic meat into chicken meat upon butchering' from Animals Logic and restart the game. Otherwise, weird bug will occur!");
            }
        }

        public override bool DetectMod()
        {
            if (!base.DetectMod()) return false;

            MeatLogger.Debug("AnimalLogic Detected!");

            var settings = Type.GetType("AnimalsLogic.Settings, AnimalsLogic");
            if (settings == null)
            {
                return false;
            }

            var fiSettings = settings.GetFields().Where(x => x.IsStatic);
            foreach (var setting in fiSettings)
            {
                if (setting is FieldInfo b && b.Name == "tastes_like_chicken" &&
                    b.GetValue(null) as bool? == true)
                {
                    return true;
                }
            }
            return false;

        }
    }

}
