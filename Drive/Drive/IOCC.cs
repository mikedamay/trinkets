
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

        private bool inited;

        private IOCC()
        {
            Init(TypeMapHolder.Instance.GetRootType());
         }

        public TRootType CreateInstance<TRootType>(Type rootType)
        {
            object root = Init(typeof(TRootType));
            if (!(root is TRootType))
            {
                throw new Exception($"object created by IOC container is not {typeof(TRootType).Name} as expected");
            }
            inited = true;
            return (TRootType)root;
        }

        private object Init(Type rootType)
        {
            object root = Construct(rootType);
            sheetProcessor = root as SheetProcessor;
            FieldInfo[] propertyInfos = rootType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.GetCustomAttribute<IOCCInjectedDependencyAttribute>() != null)
                {
                    
                    if (typeMap.ContainsKey(propertyInfo.FieldType))
                    {
                        Type implementation = typeMap[propertyInfo.FieldType];
                        object dependency = Construct(implementation);
                        propertyInfo.SetValue(root, dependency);
                    }
                    
                }
            }
            return root;
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
            var noArgConstructorInfo = constructorInfos.FirstOrDefault(ci => ci.GetParameters().Length == 0);
            if (noArgConstructorInfo == null)
            {
                throw new Exception($"There is no no-arg constructor for {rootType.Name}.  A no-arg constructor is required.");
            }
            return noArgConstructorInfo.Invoke(flags | BindingFlags.CreateInstance, null, new object[0], null);

        }

        SheetProcessor sheetProcessor;
        public SheetProcessor SheetProcessor => sheetProcessor;

        private Type GetImplementation(Type type)
        {
            return typeMap[type];
        }
    }
}
