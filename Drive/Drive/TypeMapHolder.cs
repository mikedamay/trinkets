using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.TheDisappointedProgrammer.IOC;

namespace com.TheDisappointedProgrammer.Drive
{
    internal class TypeMapHolder : IOCConfigurer
    {
        internal static TypeMapHolder Instance = new TypeMapHolder();

        public IDictionary<Type, Type> GetTypeMap() => GetStdTypeMap();
        /**
         * <return>the class (not the interface) of the top object in the object tree.
         *         all othwer dependencies to be injected must be members of this class or members of those members, ad infinitum</return>
         */
        public Type GetRootType()
        {
            return typeof(SheetProcessor);
        }

        private IDictionary<Type, Type> GetStdTypeMap()
        {
            return new Dictionary<Type, Type>()
            {
                {typeof(SheetProcessor), typeof(SheetProcessor)},
                {typeof(AppContext), typeof(StdAppContext)}
            };

    }
}
}
