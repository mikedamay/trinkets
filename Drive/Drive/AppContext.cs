using com.theDisappointedProgrammer.Drive;

namespace com.TheDisappointedProgrammer.Drive
{
    public interface AppContext
    {
        OSPlatform OsPlatform { get; }
    }
    public class StdAppContext : AppContext
    {
        [IOCCInjectedDependency]
        private OSPlatform osPlatform = new WindowsPlatform();
        public OSPlatform OsPlatform => osPlatform;
     }
}
