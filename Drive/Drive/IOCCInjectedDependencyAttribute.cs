using System;

namespace com.TheDisappointedProgrammer.Drive
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class IOCCInjectedDependencyAttribute : Attribute
    { 
    }
}