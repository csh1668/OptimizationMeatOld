using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AlienMeatTest.Compatibility
{
    public abstract class Compatibility
    {
        // TODO: Make compatibility patches more 'Object-Oriented'.
        protected abstract string PackageID { get; }
        public abstract bool IsPreOptimization { get; }

        public virtual bool DetectMod()
        {
            return ModLister.AllInstalledMods.Where(x => x.Active).Any(mods => mods.PackageId == PackageID);
        }
        public abstract void DoPatch();

        public override string ToString() => "Patch for " + PackageID;
    }
}
