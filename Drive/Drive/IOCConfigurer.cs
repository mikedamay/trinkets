using System;
using System.Collections.Generic;

namespace com.TheDisappointedProgrammer.IOC
{
    public interface IOCConfigurer
    {
        IDictionary<Type, Type> GetTypeMap();
        Type GetRootType();
    }
}
