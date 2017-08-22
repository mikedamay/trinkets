using com.theDisappointedProgrammer.Drive;
using com.TheDisappointedProgrammer.IOCC;

namespace com.TheDisappointedProgrammer.Drive
{
    public interface AppContext
    {
        OSPlatform OsPlatform { get; }
    }
    [Bean]
    public class StdAppContext : AppContext
    {
        [BeanReference]
        private OSPlatform osPlatform;
        public OSPlatform OsPlatform => osPlatform;
     }
}
