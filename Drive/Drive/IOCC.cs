
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using com.TheDisappointedProgrammer.IOC;

namespace com.TheDisappointedProgrammer.Drive
{
    public class IOCC
    {
        public static IOCC Instance { get; } = new IOCC();

        private IDictionary<Type, Type> typeMap = TypeMapHolder.Instance.GetTypeMap();

        private IOCC()
        {
            Init(TypeMapHolder.Instance.GetRootType());
        }

        private void Init(Type rootType)
        {
            object root = Construct(rootType);
            sheetProcessor = root as SheetProcessor;
            var properties = rootType.GetProperties(BindingFlags.Instance )
            if (typeMap.ContainsKey(typeof(SheetProcessor)))
            {

            }
        }
        /**
         * <summary>checks if the type to be instantiated has an empty constructor and if so constructs it</summary>
         * <param name="rootType">a concrete clasws typically part of the object tree being instantiated</param>
         * <throws>InvalidArgumentException</throws>
         */
        private object Construct(Type rootType)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var constructorInfos = rootType.GetConstructors(flags);
            var numNoArgConstructors = constructorInfos.Count(ci => ci.GetParameters().Length == 0);
            if (numNoArgConstructors == 0)
            {
                throw new Exception($"There is no no-arg constructor for {rootType.Name}.  A no-arg constructor is required.");
            }
            return constructorInfos[0].Invoke(flags | BindingFlags.CreateInstance, null, new object[0], null);
        }

        public AppContext AppContext { get; } = new StdAppContext();
        SheetProcessor sheetProcessor;
        public SheetProcessor SheetProcessor => sheetProcessor;

        private Type GetImplementation(Type type)
        {
            return typeMap[type];
        }
    }
}
