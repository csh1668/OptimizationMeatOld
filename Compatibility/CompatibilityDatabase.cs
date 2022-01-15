using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AlienMeatTest.Compatibility
{
    public static class CompatibilityDatabase
    {
        private static List<Compatibility> _list = new List<Compatibility>();

        public static void InitDatabase()
        {
            _list.Clear();
            var sMakers = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass && x.Name.StartsWith(nameof(Compatibility)) && !x.IsAbstract);
            foreach (var sMaker in sMakers)
            {
                _list.Add((Compatibility)Activator.CreateInstance(sMaker));
            }

            for (int i = _list.Count - 1; i >= 0; i--)
            {
                if (!_list[i].DetectMod())
                    _list.Remove(_list[i]);
            }
            MeatLogger.Debug("Instantiated Patches: " + string.Join(",", _list) + " Total: " + _list.Count);
        }

        public static IEnumerable<Compatibility> All => _list;

        //public static Compatibility GetNamed(string name)
        //{
        //    return _list.FirstOrDefault(maker => maker.GetType().Name == name);
        //}
    }
}
