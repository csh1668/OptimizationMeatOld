using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Verse;
using RimWorld;

namespace AlienMeatTest
{
    public class MeatListDef : Def
    {
        public List<string> meats;
        public List<string> races;
        /*
        TODO: Example of Defs folder structure
        Defs/MeatListDef/Def.xml 
        ...
        in Def.xml >> <AlienMeatTest.MeatListDef> ... <defName>WhiteList</defName> ... <lit><li>Meat_Cow</li></lst>
        Compatibility patch from other mods is much easier. More 'RW Style'.
        */
    }
}