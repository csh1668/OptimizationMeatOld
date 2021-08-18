using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace AlienMeatTest
{
    public class MeatPostOptimization
    {
        public static void PostOptimize()
        {
            MeatLogger.Debug("Post optimization start...");
            ResolveCategoryDefs();
            ResetThingSetMakerUtility();
            ResolveLanguageDataForRawMeat();

            foreach (var thingSetMakerDef in DefDatabase<ThingSetMakerDef>.AllDefs)
            {
                foreach (var childThingSetMaker in GetDescendantThingSetMakers(thingSetMakerDef.root).Concat(thingSetMakerDef.root))
                {
                    List<ThingDef> toDisallow = new List<ThingDef>();
                    if (childThingSetMaker.fixedParams.filter?.AllowedThingDefs != null)
                    {
                        foreach (var allowedThingDef in childThingSetMaker.fixedParams.filter.AllowedThingDefs)
                        {
                            if (MeatOptimization.toRemoveDefsCached.Contains(allowedThingDef.defName))
                            {
                                toDisallow.Add(allowedThingDef);
                            }
                        }

                        int b = childThingSetMaker.fixedParams.filter.AllowedDefCount;
                        foreach (var thingDef in toDisallow)
                        {
                            childThingSetMaker.fixedParams.filter.SetAllow(thingDef, false);
                        }

                        int a = childThingSetMaker.fixedParams.filter.AllowedDefCount;
                        MeatLogger.Debug(
                            $"{childThingSetMaker.fixedParams.filter.DisplayRootCategory.Label}, {b - a}");
                    }

                }
            }


            MeatLogger.Debug("Post optimization done!");
        }

        private static void ResolveCategoryDefs()
        {
            ThingCategoryDefOf.MeatRaw.ResolveReferences();
            ThingCategoryDefOf.Foods.ResolveReferences();
            ThingCategoryDefOf.Root.ResolveReferences();
        }

        private static void ResetThingSetMakerUtility()
        {
            int before = ThingSetMakerUtility.allGeneratableItems.Count;
            ThingSetMakerUtility.Reset();
            int after = ThingSetMakerUtility.allGeneratableItems.Count;
            MeatLogger.Debug($"Amount of removed from ThingSetMakerUtility.allGeneratableItems: {before - after}");
        }

        private static void ResolveLanguageDataForRawMeat()
        {
            if (!Patches.DefGeneratorPatch.PatchExecuted)
                return;
            var meatCow = ThingDef.Named("Meat_Cow");
            var rawMeatLabel = Patches.DefGeneratorPatch.RawMeatLabel;
            if (meatCow.label != rawMeatLabel)
            {
                MeatLogger.Debug(meatCow.label + " changed to " + rawMeatLabel);
                meatCow.label = Patches.DefGeneratorPatch.RawMeatLabel;
            }
            else
            {
                MeatLogger.Debug(meatCow.label + ", no need to change");
            }
        }
        private static IEnumerable<ThingSetMaker> GetDescendantThingSetMakers(ThingSetMaker parent)
        {
            if (parent == null) yield break;
            var optionsFieldInfo = parent.GetType().GetField("options");
            if (optionsFieldInfo != null)
            {
                var options = optionsFieldInfo.GetValue(parent) as IEnumerable;
                if (options == null) yield break;
                foreach (var option in options)
                {
                    var childThingSetMakerFieldInfo = option.GetType().GetField("thingSetMaker");
                    var childThingSetMaker = childThingSetMakerFieldInfo.GetValue(option) as ThingSetMaker;

                    yield return childThingSetMaker;
                    foreach (var temp in GetDescendantThingSetMakers(childThingSetMaker))
                    {
                        yield return temp;
                    }
                }
            }
        }

    }
}
