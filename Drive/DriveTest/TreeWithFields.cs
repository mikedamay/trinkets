using com.TheDisappointedProgrammer.Drive;

namespace IOCCTest
{
    public class TreeWithFields
    {
        [IOCCInjectedDependency]
        public ChildOne childOne;
    }

    public class ChildOne
    {
        
    }
}