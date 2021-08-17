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
        private static List<string> impliedMeats = new List<string>();
        // Todo: Use Transpiler for the cool code
        public static IEnumerable<ThingDef> Postfix(IEnumerable<ThingDef> values)
        {
            PatchExecuted = true;
            foreach (var thingDef in values)
            {
                //Log.Message(thingDef.defName);
                impliedMeats.Add(thingDef.defName);
                yield return thingDef;
            }
        }
    }

    [HarmonyPatch(typeof(ThingIDMaker)), HarmonyPatch("GiveIDTo")]
    class PatchTest
    {
        static bool Prefix(Thing t)
        {
            if (!t.def.HasThingIDNumber)
            {
                return false;
            }
            if (t.thingIDNumber != -1)
            {
                Log.Error(string.Concat(new object[]
                {
                    "Giving ID to ",
                    t,
                    " which already has id ",
                    t.thingIDNumber
                }));
            }

            return false;
        }
    }
}
