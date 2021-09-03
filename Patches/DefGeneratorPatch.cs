using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using Verse;

namespace AlienMeatTest.Patches
{
    [HarmonyPatch(typeof(DefGenerator)), HarmonyPatch("GenerateImpliedDefs_PreResolve")]
    public class DefGeneratorPatch
    {
        // ReSharper disable once IdentifierTypo
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // ThingDefGenerator_Meat.ImpliedMeatDefs 호출을 DefGeneratorPatch.ImpliedMeatDefsNew 호출로 바꾸기
            var Method_before = AccessTools.Method(typeof(ThingDefGenerator_Meat), "ImpliedMeatDefs");
            var Method_after = AccessTools.Method(typeof(DefGeneratorPatch), "ImpliedMeatDefsNew");
            foreach (var codeInstruction in instructions)
            {
                if (codeInstruction.Calls(Method_before))
                {
                    yield return new CodeInstruction(OpCodes.Call, Method_after);
                    continue;
                }

                yield return codeInstruction;
            }
        }

        public static FieldInfo dic = typeof(DefDatabase<ThingDef>).GetField("defsByName",
            BindingFlags.Static | BindingFlags.NonPublic);
        public static bool PatchExcuted { get; private set; }

        public static List<string> WhiteListDefNames { get; set; } = new List<string>
        {
            "Cow", "Human", "Megaspider"
        };
        public static IEnumerable<ThingDef> ImpliedMeatDefsNew()
        {
            PatchExcuted = true;

            bool IsRace(ThingDef thingDef) =>
                thingDef.category == ThingCategory.Pawn && thingDef.race.useMeatFrom == null &&
                thingDef.race.specificMeatDef == null;
            MeatLogger.Debug($"Hello");

            List<ThingDef> baseMeats = new List<ThingDef>();
            WhiteListDefNames.ForEach(x => baseMeats.Add(MakeNewOrGetMeat(ThingDef.Named(x))));

            var defsByName = (dic.GetValue(null) as Dictionary<string, ThingDef>);
            // baseMeats.AddRange(Settings.WhitelistMeats);

            foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.ToList())
            {
                if (IsRace(thingDef) == false) continue;

                if (thingDef.race.IsFlesh == false)
                {
                    DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(thingDef.race, "meatDef", "Steel", null, null);
                }
                else
                {
                    if (thingDef.race.Humanlike)
                    {
                        thingDef.race.meatDef = baseMeats.FirstOrDefault(x => x.defName == "Meat_Human");
                    }
                    else if (thingDef.race.FleshType == FleshTypeDefOf.Insectoid)
                    {
                        thingDef.race.meatDef = baseMeats.FirstOrDefault(x => x.defName == "Meat_Megaspider");
                    }
                    else
                    {
                        thingDef.race.meatDef = baseMeats.FirstOrDefault(x => x.defName == "Meat_Cow");
                    }

                    if (!WhiteListDefNames.Contains(thingDef.defName) &&
                        !defsByName.TryGetValue("Meat_" + thingDef, out _))
                    {
                        defsByName.Add("Meat_" + thingDef, thingDef.race.meatDef);
                    }
                }
            }
            MeatLogger.Debug($"World");

            MeatLogger.Debugs(baseMeats.Select(x=> x.defName));

            return baseMeats;
        }

        public static ThingDef MakeNewOrGetMeat(ThingDef thingDef)
        {
            ThingDef newMeat = new ThingDef();
            newMeat.drawerType = DrawerType.MapMeshOnly;
            newMeat.resourceReadoutPriority = ResourceCountPriority.Middle;
            newMeat.category = ThingCategory.Item;
            newMeat.thingClass = typeof(ThingWithComps);
            newMeat.graphicData = new GraphicData();
            newMeat.graphicData.graphicClass = typeof(Graphic_StackCount);
            newMeat.useHitPoints = true;
            newMeat.selectable = true;
            newMeat.SetStatBaseValue(StatDefOf.MaxHitPoints, 100f);
            newMeat.altitudeLayer = AltitudeLayer.Item;
            newMeat.stackLimit = 75;
            newMeat.comps.Add(new CompProperties_Forbiddable());
            CompProperties_Rottable compProperties_Rottable = new CompProperties_Rottable();
            compProperties_Rottable.daysToRotStart = 2f;
            compProperties_Rottable.rotDestroys = true;
            newMeat.comps.Add(compProperties_Rottable);
            newMeat.tickerType = TickerType.Rare;
            newMeat.SetStatBaseValue(StatDefOf.Beauty, -4f);
            newMeat.alwaysHaulable = true;
            newMeat.rotatable = false;
            newMeat.pathCost = DefGenerator.StandardItemPathCost;
            newMeat.drawGUIOverlay = true;
            newMeat.socialPropernessMatters = true;
            newMeat.modContentPack = thingDef.modContentPack;
            newMeat.category = ThingCategory.Item;
            if (thingDef.race.Humanlike)
            {
                newMeat.description = "MeatHumanDesc".Translate(thingDef.label);
            }
            else if (thingDef.race.FleshType == FleshTypeDefOf.Insectoid)
            {
                newMeat.description = "MeatInsectDesc".Translate(thingDef.label);
            }
            else
            {
                newMeat.description = "MeatDesc".Translate(thingDef.label);
            }
            newMeat.useHitPoints = true;
            newMeat.healthAffectsPrice = false;
            newMeat.SetStatBaseValue(StatDefOf.MaxHitPoints, 60f);
            newMeat.SetStatBaseValue(StatDefOf.DeteriorationRate, 6f);
            newMeat.SetStatBaseValue(StatDefOf.Mass, 0.03f);
            newMeat.SetStatBaseValue(StatDefOf.Flammability, 0.5f);
            newMeat.SetStatBaseValue(StatDefOf.Nutrition, 0.05f);
            newMeat.SetStatBaseValue(StatDefOf.FoodPoisonChanceFixedHuman, 0.02f);
            newMeat.BaseMarketValue = thingDef.race.meatMarketValue;
            if (newMeat.thingCategories == null)
            {
                newMeat.thingCategories = new List<ThingCategoryDef>();
            }
            DirectXmlCrossRefLoader.RegisterListWantsCrossRef<ThingCategoryDef>(newMeat.thingCategories, "MeatRaw", newMeat, null);
            newMeat.ingestible = new IngestibleProperties();
            newMeat.ingestible.parent = newMeat;
            newMeat.ingestible.foodType = FoodTypeFlags.Meat;
            newMeat.ingestible.preferability = FoodPreferability.RawBad;
            DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(newMeat.ingestible, "tasteThought", ThoughtDefOf.AteRawFood.defName, null, null);
            newMeat.ingestible.ingestEffect = EffecterDefOf.EatMeat;
            newMeat.ingestible.ingestSound = SoundDefOf.RawMeat_Eat;
            newMeat.ingestible.specialThoughtDirect = thingDef.race.FleshType.ateDirect;
            newMeat.ingestible.specialThoughtAsIngredient = thingDef.race.FleshType.ateAsIngredient;
            if (thingDef.ingredient != null)
            {
                newMeat.ingredient = new IngredientProperties();
                newMeat.ingredient.mergeCompatibilityTags.AddRange(thingDef.ingredient.mergeCompatibilityTags);
            }
            newMeat.graphicData.color = thingDef.race.meatColor;
            if (thingDef.race.Humanlike)
            {
                newMeat.graphicData.texPath = "Things/Item/Resource/MeatFoodRaw/Meat_Human";
            }
            else if (thingDef.race.FleshType == FleshTypeDefOf.Insectoid)
            {
                newMeat.graphicData.texPath = "Things/Item/Resource/MeatFoodRaw/Meat_Insect";
            }
            else if (thingDef.race.baseBodySize < 0.7f)
            {
                newMeat.graphicData.texPath = "Things/Item/Resource/MeatFoodRaw/Meat_Small";
            }
            else
            {
                newMeat.graphicData.texPath = "Things/Item/Resource/MeatFoodRaw/Meat_Big";
            }
            newMeat.defName = "Meat_" + thingDef.defName;
            if (thingDef.race.meatLabel.NullOrEmpty())
            {
                newMeat.label = "MeatLabel".Translate(thingDef.label);
            }
            else
            {
                newMeat.label = thingDef.race.meatLabel;
            }
            newMeat.ingestible.sourceDef = thingDef;
            thingDef.race.meatDef = newMeat;

            return newMeat;
        }
    }

}
