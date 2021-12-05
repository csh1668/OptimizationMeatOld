using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using Verse;

namespace AlienMeatTest.Patches
{
    [HarmonyPatch(typeof(ThingDefGenerator_Meat)), HarmonyPatch("ImpliedMeatDefs")]
    public class DefGeneratorPatch
    {
        public static IEnumerable<string> ImpliedMeats => impliedMeats;

        public static bool PatchExecuted { get; private set; } = false;
        public static string RawMeatLabel { get; private set; } = string.Empty;
        private static List<string> impliedMeats = new List<string>();
        // Todo: Use Transpiler for the cool code
        public static IEnumerable<ThingDef> Postfix(IEnumerable<ThingDef> values)
        {
            foreach (var thingDef in values)
            {
                if (thingDef.defName == "Meat_Cow")
                    RawMeatLabel = thingDef.label;
                impliedMeats.Add(thingDef.defName);
                yield return thingDef;
            }
            PatchExecuted = true;
        }
    }
}