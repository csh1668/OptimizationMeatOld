using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienMeatTest.Compatibility
{
    public class Compatibility_ARimworldOfMagic : Compatibility
    {
        protected override string PackageID => "torann.arimworldofmagic";
        public override bool IsPreOptimization => true;
        public override void DoPatch()
        {
            if (!DetectMod()) return;
            MeatLogger.Debug("A Rimworld of Magic Detected!");

            MeatOptimization.WhiteListMeat.Add("RawMagicyte");
        }
    }
}
