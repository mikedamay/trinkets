
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace com.TheDisappointedProgrammer.Drive
{
    public class IOCC
    {
        public static IOCC Instance { get; } = new IOCC();

        private IDictionary<Type, Type> typeMap = new Dictionary<Type, Type>()
        {
            {typeof(SheetProcessor), typeof(SheetProcessor)},
            {typeof(AppContext), typeof(StdAppContext)}
        };
        public void Init()
        {
            if (typeMap.ContainsKey(typeof(SheetProcessor)))
            {

            }
        }
        public AppContext AppContext { get; } = new StdAppContext();
        public SheetProcessor SheetProcessor { get; } = new SheetProcessor();

        private Type GetImplementation(Type type)
        {
            return typeMap[type];
        }
    }
}
