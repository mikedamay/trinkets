
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
            PropertyInfo[] propertyInfos = rootType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.GetCustomAttribute<IOCCInjectedDependencyAttribute>() != null)
                {
                    
                    if (typeMap.ContainsKey(propertyInfo.PropertyType))
                    {
                        Type implementation = typeMap[propertyInfo.PropertyType];
                        object dependency = Construct(implementation);
                        propertyInfo.SetValue(root, dependency);
                    }
                    
                }
            }
        }
        /**
         * <summary>checks if the type to be instantiated has an empty constructor and if so constructs it</summary>
         * <param name="rootType">a concrete clasws typically part of the object tree being instantiated</param>
         * <throws>InvalidArgumentException</throws>
         */
        private object Construct(Type rootType)
        {
            IEnumerator<ConstructorInfo> enumor = null;
            try
            {
                BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
                var constructorInfos = rootType.GetConstructors(flags);
                var noArgConstructorInfos = constructorInfos.Where(ci => ci.GetParameters().Length == 0).Select(ci => ci);
                if (noArgConstructorInfos.Count() == 0)
                {
                    throw new Exception($"There is no no-arg constructor for {rootType.Name}.  A no-arg constructor is required.");
                }
                enumor = noArgConstructorInfos.GetEnumerator();
                enumor.MoveNext();
                return enumor.Current.Invoke(flags | BindingFlags.CreateInstance, null, new object[0], null);

            }
            finally
            {
                if (enumor != null)
                {
                    enumor.Dispose();
                    
                }
            }
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
