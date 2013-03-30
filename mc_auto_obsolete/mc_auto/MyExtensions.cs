using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        /// <summary>
        /// pass in a default parameter to a Dictionary Get operation.
        /// </summary>
        /// <returns>The value in the dictionary, if any.  If none, then the defaultval passed in.</returns>
        public static TVALUE GetValueOrDefault<TKEY, TVALUE>(this IDictionary<TKEY, TVALUE> map
          , TKEY key, TVALUE defaultval)
        {
            TVALUE gotval;
            if (!map.TryGetValue(key, out gotval))
            {
                gotval = defaultval;
            }
            return gotval;
        }
    }
}
