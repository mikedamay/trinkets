using com.theDisappointedProgrammer.Drive;

namespace com.TheDisappointedProgrammer.Drive
{
    public interface AppContext
    {
        Platform Platform { get; }
    }
    public class StdAppContext : AppContext
    {
        public Platform Platform { get; } = new WindowsPlatform();
    }
}
