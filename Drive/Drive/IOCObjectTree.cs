﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace com.TheDisappointedProgrammer.Drive
{
    class IOCObjectTree
    {
        private IDictionary<Type, Type> typeMap = TypeMapHolder.Instance.GetTypeMap();
        // TODO complete the documentation item 3 below if and when factory types are implemented
        /// <summary>
        /// 1. mainly used to create the complete object tree at program startup
        /// 2. may be used to create object tree fragments when running tests
        /// 3. may be used to create an object or link to an existing object
        /// </summary>
        /// <typeparam name="TRootType">The concrete class (not an interface) of the top object in the tree</typeparam>
        /// <returns>an ojbect of root type</returns>
        public TRootType GetOrCreateObjectTree<TRootType>()
        {
            object rootObject = CreateObjectTree(typeof(TRootType));
            if (!(rootObject is TRootType))
            {
                throw new Exception($"object created by IOC container is not {typeof(TRootType).Name} as expected");
            }
            return (TRootType)rootObject;
        }

        /// <summary>
        /// see documentation for GetOrCreateObjectTree
        /// </summary>
        private object CreateObjectTree(Type rootType)
        {
            object rootObject = Construct(rootType);
            FieldInfo[] propertyInfos = rootType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.GetCustomAttribute<IOCCInjectedDependencyAttribute>() != null)
                {

                    if (typeMap.ContainsKey(propertyInfo.FieldType))
                    {
                        Type implementation = typeMap[propertyInfo.FieldType];
                        object dependency = Construct(implementation);
                        propertyInfo.SetValue(rootObject, dependency);
                    }

                }
            }
            return rootObject;
        }
        ///
        /// <summary>checks if the type to be instantiated has an empty constructor and if so constructs it</summary>
        /// <param name="rootType">a concrete clasws typically part of the object tree being instantiated</param>
        /// <exception>InvalidArgumentException</exception>  
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
    }
}
