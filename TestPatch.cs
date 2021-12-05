using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using ProjectRimFactory.SAL3.Things.Assemblers;
using RimWorld;
using Verse;

namespace AlienMeatTest
{
    //[HarmonyPatch(typeof(Building_ProgrammableAssembler)), HarmonyPatch("ProduceItems")]
    public static class TestPatch
    {
        public static void Prefix(RecipeDef recipeDef)
        {
            MeatLogger.Message(recipeDef?.defName ?? "null");
            MeatLogger.Messages(recipeDef.products.Select(x=> x.thingDef.defName));
        }
    }
}
