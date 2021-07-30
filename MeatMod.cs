using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using RimWorld;
using UnityEngine;

namespace AlienMeatTest
{
    public class MeatMod : Mod
    {
        private MeatModSettings settings;
        public MeatMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<MeatModSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            settings.DoWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "OM_Category".Translate();
        }
    }
}
