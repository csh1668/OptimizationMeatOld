using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace AlienMeatTest.Patches
{
    public static class MeatOptimizerUtility
    {
		//public static IEnumerable<ThingDef> ImpliedMeatDefsNew()
		//{
		//	foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.ToList())
		//	{
		//		if (thingDef.category == ThingCategory.Pawn && thingDef.race.useMeatFrom == null && thingDef.race.specificMeatDef == null)
		//		{
		//			if (!thingDef.race.IsFlesh)
		//			{
		//				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(thingDef.race, "meatDef", "Steel");
		//			}
		//			else
		//			{
		//				ThingDef thingDef2 = new ThingDef();
		//				thingDef2.drawerType = DrawerType.MapMeshOnly;
		//				thingDef2.resourceReadoutPriority = ResourceCountPriority.Middle;
		//				thingDef2.category = ThingCategory.Item;
		//				thingDef2.thingClass = typeof(ThingWithComps);
		//				thingDef2.graphicData = new GraphicData();
		//				thingDef2.graphicData.graphicClass = typeof(Graphic_StackCount);
		//				thingDef2.useHitPoints = true;
		//				thingDef2.selectable = true;
		//				thingDef2.SetStatBaseValue(StatDefOf.MaxHitPoints, 100f);
		//				thingDef2.altitudeLayer = AltitudeLayer.Item;
		//				thingDef2.stackLimit = 75;
		//				thingDef2.comps.Add(new CompProperties_Forbiddable());
		//				CompProperties_Rottable compProperties_Rottable = new CompProperties_Rottable();
		//				compProperties_Rottable.daysToRotStart = 2f;
		//				compProperties_Rottable.rotDestroys = true;
		//				thingDef2.comps.Add(compProperties_Rottable);
		//				thingDef2.tickerType = TickerType.Rare;
		//				thingDef2.SetStatBaseValue(StatDefOf.Beauty, -4f);
		//				thingDef2.alwaysHaulable = true;
		//				thingDef2.rotatable = false;
		//				thingDef2.pathCost = DefGenerator.StandardItemPathCost;
		//				thingDef2.drawGUIOverlay = true;
		//				thingDef2.socialPropernessMatters = true;
		//				thingDef2.modContentPack = thingDef.modContentPack;
		//				thingDef2.category = ThingCategory.Item;
		//				if (thingDef.race.Humanlike)
		//				{
		//					thingDef2.description = "MeatHumanDesc".Translate(thingDef.label);
		//				}
		//				else if (thingDef.race.FleshType == FleshTypeDefOf.Insectoid)
		//				{
		//					thingDef2.description = "MeatInsectDesc".Translate(thingDef.label);
		//				}
		//				else
		//				{
		//					thingDef2.description = "MeatDesc".Translate(thingDef.label);
		//				}
		//				thingDef2.useHitPoints = true;
		//				thingDef2.healthAffectsPrice = false;
		//				thingDef2.SetStatBaseValue(StatDefOf.MaxHitPoints, 60f);
		//				thingDef2.SetStatBaseValue(StatDefOf.DeteriorationRate, 6f);
		//				thingDef2.SetStatBaseValue(StatDefOf.Mass, 0.03f);
		//				thingDef2.SetStatBaseValue(StatDefOf.Flammability, 0.5f);
		//				thingDef2.SetStatBaseValue(StatDefOf.Nutrition, 0.05f);
		//				thingDef2.SetStatBaseValue(StatDefOf.FoodPoisonChanceFixedHuman, 0.02f);
		//				thingDef2.BaseMarketValue = thingDef.race.meatMarketValue;
		//				if (thingDef2.thingCategories == null)
		//				{
		//					thingDef2.thingCategories = new List<ThingCategoryDef>();
		//				}
		//				DirectXmlCrossRefLoader.RegisterListWantsCrossRef<ThingCategoryDef>(thingDef2.thingCategories, "MeatRaw", thingDef2, null);
		//				thingDef2.ingestible = new IngestibleProperties();
		//				thingDef2.ingestible.parent = thingDef2;
		//				thingDef2.ingestible.foodType = FoodTypeFlags.Meat;
		//				thingDef2.ingestible.preferability = FoodPreferability.RawBad;
		//				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(thingDef2.ingestible, "tasteThought", ThoughtDefOf.AteRawFood.defName, null, null);
		//				thingDef2.ingestible.ingestEffect = EffecterDefOf.EatMeat;
		//				thingDef2.ingestible.ingestSound = SoundDefOf.RawMeat_Eat;
		//				thingDef2.ingestible.specialThoughtDirect = thingDef.race.FleshType.ateDirect;
		//				thingDef2.ingestible.specialThoughtAsIngredient = thingDef.race.FleshType.ateAsIngredient;
		//				if (thingDef.ingredient != null)
		//				{
		//					thingDef2.ingredient = new IngredientProperties();
		//					thingDef2.ingredient.mergeCompatibilityTags.AddRange(thingDef.ingredient.mergeCompatibilityTags);
		//				}
		//				thingDef2.graphicData.color = thingDef.race.meatColor;
		//				if (thingDef.race.Humanlike)
		//				{
		//					thingDef2.graphicData.texPath = "Things/Item/Resource/MeatFoodRaw/Meat_Human";
		//				}
		//				else if (thingDef.race.FleshType == FleshTypeDefOf.Insectoid)
		//				{
		//					thingDef2.graphicData.texPath = "Things/Item/Resource/MeatFoodRaw/Meat_Insect";
		//				}
		//				else if (thingDef.race.baseBodySize < 0.7f)
		//				{
		//					thingDef2.graphicData.texPath = "Things/Item/Resource/MeatFoodRaw/Meat_Small";
		//				}
		//				else
		//				{
		//					thingDef2.graphicData.texPath = "Things/Item/Resource/MeatFoodRaw/Meat_Big";
		//				}
		//				thingDef2.defName = "Meat_" + thingDef.defName;
		//				if (thingDef.race.meatLabel.NullOrEmpty())
		//				{
		//					thingDef2.label = "MeatLabel".Translate(thingDef.label);
		//				}
		//				else
		//				{
		//					thingDef2.label = thingDef.race.meatLabel;
		//				}
		//				thingDef2.ingestible.sourceDef = thingDef;
		//				thingDef.race.meatDef = thingDef2;
		//				yield return thingDef2;
		//			}
		//		}
		//	}
		//}
		public static ThingDef MakeNewMeat(ThingDef sourceAnimal)
        {
            return new ThingDef();
        }
    }
}
