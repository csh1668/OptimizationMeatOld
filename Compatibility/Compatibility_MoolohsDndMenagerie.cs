using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/** Certain animals from Mooloh's DND Menagerie drop non-meat items (namely steel and sandstone). We need to whitelist these not-really-meat defs to avoid having the 
 * optimizer try to remove these defs from the game.
 * */
namespace AlienMeatTest.Compatibility
{
    public class Compatibility_MoolohsDndMenagerie : Compatibility
    {
        protected override string PackageID => "mooloh.dndmenagerie";
        public override bool IsPreOptimization => true;
        public override void DoPatch()
        {
            if (!DetectMod()) return;
            MeatLogger.Debug("Mooloh's DND Menagerie!");

            MeatOptimization.WhiteListMeat.Add("Steel");
            MeatOptimization.WhiteListMeat.Add("BlocksSandstone");
        }
    }
}
