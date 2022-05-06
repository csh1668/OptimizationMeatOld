using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/** Certain animals from Mooloh's DND Menagerie drop non-meat items (namely steel and sandstone). We need to whitelist these not-really-meat-dropping animals
 * so this mod doesn't think that steel and stone are meat and bork the game trying to reconcile that idea.
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
