using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AlienMeatTest.Compatibility
{
    //public class Compatibility_WarhammerSkaven : Compatibility
    //{
    //    protected override string PackageID => "kompadt.warhammer.skaven";
    //    public override bool IsPreOptimization { get; }
    //    public override void DoPatch()
    //    {
    //        if (!DetectMod()) return;
    //        MeatLogger.Debug("Warhammer: Skaven Detected!");

    //        var settings = LoadedModManager.GetMod<MeatMod>().GetSettings<MeatModSettings>();
    //        const string skavenMeat = "Meat_Alien_Skaven";

    //        if (!settings.OptimizationAnimalMeat) return;
    //        var humanMeatDef = ThingDef.Named("Meat_Human");
    //        var racesThatDropSkavenMeat =
    //            DefDatabase<ThingDef>.AllDefs.Where(x => x.race?.meatDef?.defName == skavenMeat).ToList();
    //        foreach (var i in racesThatDropSkavenMeat)
    //        {
    //            var defName = i.defName;
    //            MeatOptimization.WhiteListRace.Add(defName);
    //            i.race.meatDef = humanMeatDef;
    //        }
    //        MeatOptimization.RemovedMeatDefs.Add(ThingDef.Named(skavenMeat).defName);
    //    }
    //}
}
