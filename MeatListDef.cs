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
        public List<string> lst;


        public bool IsValidated() => lst[0] == "Meat_Cow" && lst[1] == "Meat_Human" && lst[2] == "Meat_Megaspider";
        internal static MeatListDef Base => new MeatListDef() 
        {
            lst = new List<string>()
            {
                "Meat_Cow", "Meat_Human", "Meat_Megaspider"
            };
        }
        /*
        TODO: Example of Defs folder structure
        Defs/MeatListDef/Def.xml 
        ...
        in Def.xml >> <AlienMeatTest.MeatListDef> ... <defName>WhiteList</defName> ... <lit><li>Meat_Cow</li></lst>
        Compatibility patch from other mods is much easier. More 'RW Style'.
        */
    }
}