using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AlienMeatTest
{
    public static class MeatLogger
    {
        private static MeatModSettings settings = LoadedModManager.GetMod<MeatMod>().GetSettings<MeatModSettings>();
        private static StringBuilder sb = new StringBuilder();

        public static void Message(string str)
        {
            Verse.Log.Message(SeoHyeon.MOD_NAME_COLORED + ": " + str);
        }

        public static void Error(string str)
        {
            Verse.Log.Error(SeoHyeon.MOD_NAME_COLORED + ": " + str);
        }

        public static void Debug(string str)
        {
            if(settings.debugMode)
                Message($"<color=green>DEBUG</color> | " + str);
        }

        public static void Debug()
        {
            Debug(sb.ToString());
        }

        public static void DebugStack(string str)
        {
            sb.Append(str + ", ");
        }

        public static void Messages(IEnumerable enumerable)
        {
            foreach (var i in enumerable)
            {
                if (i is Def def)
                {
                    Message(def.defName);
                }
                else
                {
                    Message(i.ToString());
                }
            }
        }

        public static void Message(object o)
        {
            string str = o != null ? o.ToString() : "null";
            Message(str);
        }

        public static void Debug(object o)
        {
            string str = o != null ? o.ToString() : "null";
            Debug(str);
        }
    }
}
