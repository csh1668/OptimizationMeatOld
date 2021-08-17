using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AlienMeatTest
{
    internal static class MeatLogger
    {
        private static StringBuilder sb = new StringBuilder();
        internal static bool debugMode = false;

        internal static void Message(string str)
        {
            Verse.Log.Message(SeoHyeon.MOD_NAME_COLORED + ": " + str);
        }

        internal static void Error(string str)
        {
            Verse.Log.Error(SeoHyeon.MOD_NAME_COLORED + ": " + str);
        }

        internal static void Warn(string str)
        {
            Verse.Log.Warning(SeoHyeon.MOD_NAME_COLORED + ": " + str);
        }

        internal static void Debug(string str)
        {
            if(debugMode)
                Message($"<color=green>DEBUG</color> | " + str);
        }

        internal static void Debug()
        {
            Debug(sb.ToString());
            sb.Clear();
        }

        internal static void DebugEnumerate(string str)
        {
            sb.Append(str + ", ");
        }

        internal static void Debugs(IEnumerable<string> contents)
        {
            if (contents == null) return;
            foreach (var content in contents)
            {
                DebugEnumerate(content);
            }
            Debug();
        }

        internal static void Messages(IEnumerable enumerable)
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

        internal static void Message(object o)
        {
            string str = o != null ? o.ToString() : "null";
            Message(str);
        }

        internal static void Debug(object o)
        {
            string str = o != null ? o.ToString() : "null";
            Debug(str);
        }
    }
}
