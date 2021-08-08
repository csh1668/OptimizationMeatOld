using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;

namespace AlienMeatTest
{
    public static class MeatOptimization
    {
        private static MeatModSettings settings = LoadedModManager.GetMod<MeatMod>().GetSettings<MeatModSettings>();
        public static int OptimizeMeat()
        {
            // Get all of ThingDefs
            var defs = DefDatabase<ThingDef>.AllDefs;

            // Meats that we won't remove
            var cowMeatDef = ThingDef.Named("Meat_Cow");
            var humanMeatDef = ThingDef.Named("Meat_Human");
            var insectMeatDef = ThingDef.Named("Meat_Megaspider");


            // Meats that need to be removed from the game
            List<string> toRemoveDefs = new List<string>();

            // These ingredients(thingdefs) must not be removed
            List<string> singleIngredients = GetSingleIngredients();

            foreach (var thingDef in defs)
            {
                if (thingDef.race == null || thingDef.race.IsMechanoid)
                    continue;

                if (thingDef.race.useMeatFrom != null)
                {
                    var useMeatFrom = thingDef.race.useMeatFrom.defName;
                    // Already optimized
                    if (useMeatFrom == "Human" || useMeatFrom == "Cow" || useMeatFrom == "Megaspider" ||
                        useMeatFrom == "Steel")
                        continue;
                }

                if (thingDef.race.meatDef == null || singleIngredients.Contains(thingDef.race.meatDef.defName))
                    continue;

                if (thingDef.race.Humanlike)
                {
                    if (settings.OptimizationAlienMeat == false)
                        continue;
                    toRemoveDefs.Add(thingDef.race.meatDef.defName.Clone() as string);
                    thingDef.race.meatDef = humanMeatDef;
                }
                else if (thingDef.race.FleshType == FleshTypeDefOf.Insectoid)
                {
                    if (settings.OptimizationAnimalMeat == false)
                        continue;
                    toRemoveDefs.Add(thingDef.race.meatDef.defName.Clone() as string);
                    thingDef.race.meatDef = insectMeatDef;
                }
                else
                {
                    if (settings.OptimizationAnimalMeat == false)
                        continue;
                    toRemoveDefs.Add(thingDef.race.meatDef.defName.Clone() as string);
                    thingDef.race.meatDef = cowMeatDef;
                }
            }

            toRemoveDefs = toRemoveDefs.Distinct().ToList();

            foreach (var removeDef in toRemoveDefs)
            {
                MeatLogger.DebugEnumerate(removeDef);
            }
            MeatLogger.DebugEnumerate(toRemoveDefs.Count.ToString()); MeatLogger.Debug();

            RemoveDefs(toRemoveDefs);

            //DefDatabase<ThingDef>.ResolveAllReferences();

            //DefGenerator.AddImpliedDef(MakeNewRawMeat());

            return toRemoveDefs.Count;
        }

        public static void PostOptimize()
        {
            ThingCategoryDefOf.MeatRaw.ResolveReferences();
            ThingCategoryDefOf.Foods.ResolveReferences();
            ThingCategoryDefOf.Root.ResolveReferences();

            int before = ThingSetMakerUtility.allGeneratableItems.Count;
            ThingSetMakerUtility.Reset();
            int after = ThingSetMakerUtility.allGeneratableItems.Count;
            MeatLogger.Debug($"Amount of removed from ThingSetMakerUtility.allGeneratableItems: {before - after}");
        }

        private static List<string> GetSingleIngredients()
        {
            var recipes = DefDatabase<RecipeDef>.AllDefs;

            List<string> singleIngredients = new List<string>();
            foreach (var recipeDef in recipes)
            {
                foreach (var ingredient in recipeDef.ingredients)
                {
                    if (ingredient.filter == null) continue;
                    if (ingredient.filter.AllowedDefCount <= 1 &&
                        ingredient.filter.AnyAllowedDef != null &&
                        ingredient.filter.AnyAllowedDef.IsMeat)
                    {
                        singleIngredients.Add(ingredient.filter.AnyAllowedDef.defName.Clone() as string);
                    }
                }
            }

            return singleIngredients.Distinct().ToList();
        }

        private static void RemoveDefs(List<string> toRemoveDefs)
        {
            MethodInfo removeMethod = typeof(DefDatabase<ThingDef>).GetMethod("Remove", BindingFlags.Static | BindingFlags.NonPublic);
            foreach (var removeDef in toRemoveDefs)
            {
                if (removeDef == "Meat_Cow" ||
                    removeDef == "Meat_Human" ||
                    removeDef == "Meat_Megaspider" ||
                    removeDef == "Steel")
                {
                    // Don't Remove
                    continue;
                }
                // Remove def from ThingCategory
                ThingCategoryDefOf.MeatRaw.childThingDefs.Remove(ThingDef.Named(removeDef));
                // Remove def from DefDatabase
                removeMethod?.Invoke(null, new object[] { ThingDef.Named(removeDef) });
            }
        }

        private static ThingDef MakeNewRawMeat()
        {
            var cow = ThingDef.Named("Cow");
            ThingDef rawMeat = new ThingDef();
            rawMeat.drawerType = DrawerType.MapMeshOnly;
            rawMeat.resourceReadoutPriority = ResourceCountPriority.Middle;
            rawMeat.category = ThingCategory.Item;
            rawMeat.thingClass = typeof(ThingWithComps);
            rawMeat.graphicData = new GraphicData();
            rawMeat.graphicData.graphicClass = typeof(Graphic_StackCount);
            rawMeat.useHitPoints = true;
            rawMeat.selectable = true;
            rawMeat.SetStatBaseValue(StatDefOf.MaxHitPoints, 100f);
            rawMeat.altitudeLayer = AltitudeLayer.Item;
            rawMeat.stackLimit = 75;
            rawMeat.comps.Add(new CompProperties_Forbiddable());
            CompProperties_Rottable compProperties_Rottable = new CompProperties_Rottable();
            compProperties_Rottable.daysToRotStart = 2f;
            compProperties_Rottable.rotDestroys = true;
            rawMeat.comps.Add(compProperties_Rottable);
            rawMeat.tickerType = TickerType.Rare;
            rawMeat.SetStatBaseValue(StatDefOf.Beauty, -4f);
            rawMeat.alwaysHaulable = true;
            rawMeat.rotatable = false;
            rawMeat.pathCost = DefGenerator.StandardItemPathCost;
            rawMeat.drawGUIOverlay = true;
            rawMeat.socialPropernessMatters = true;
            rawMeat.modContentPack = cow.modContentPack;
            rawMeat.category = ThingCategory.Item;
            rawMeat.description = "MeatDesc".Translate(cow.label);
            rawMeat.useHitPoints = true;
            rawMeat.healthAffectsPrice = false;
            rawMeat.SetStatBaseValue(StatDefOf.MaxHitPoints, 60f);
            rawMeat.SetStatBaseValue(StatDefOf.DeteriorationRate, 6f);
            rawMeat.SetStatBaseValue(StatDefOf.Mass, 0.03f);
            rawMeat.SetStatBaseValue(StatDefOf.Flammability, 0.5f);
            rawMeat.SetStatBaseValue(StatDefOf.Nutrition, 0.05f);
            rawMeat.SetStatBaseValue(StatDefOf.FoodPoisonChanceFixedHuman, 0.02f);
            rawMeat.BaseMarketValue = cow.race.meatMarketValue;
            DirectXmlCrossRefLoader.RegisterListWantsCrossRef<ThingCategoryDef>(rawMeat.thingCategories, "MeatRaw", rawMeat, null);
            rawMeat.ingestible = new IngestibleProperties();
            rawMeat.ingestible.parent = rawMeat;
            rawMeat.ingestible.foodType = FoodTypeFlags.Meat;
            rawMeat.ingestible.preferability = FoodPreferability.RawBad;
            DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(rawMeat.ingestible, "tasteThought", ThoughtDefOf.AteRawFood.defName, null, null);
            rawMeat.ingestible.ingestEffect = EffecterDefOf.EatMeat;
            rawMeat.ingestible.ingestSound = SoundDefOf.RawMeat_Eat;
            rawMeat.ingestible.specialThoughtDirect = cow.race.FleshType.ateDirect;
            rawMeat.ingestible.specialThoughtAsIngredient = cow.race.FleshType.ateAsIngredient;
            rawMeat.graphicData.color = cow.race.meatColor;
            rawMeat.graphicData.texPath = "Things/Item/Resource/MeatFoodRaw/Meat_Big";
            rawMeat.defName = "Meat_Raw";
            rawMeat.label = "raw meat";
            //rawMeat.ingestible.sourceDef
            return rawMeat;
        }
    }
}