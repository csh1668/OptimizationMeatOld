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
    [HarmonyPatch(typeof(DefDatabase<ThingDef>), "GetNamed")]
    public static class DefDatabasePatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) 
        {
            return instructions;
        }
    }
}
