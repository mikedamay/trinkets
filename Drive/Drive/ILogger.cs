namespace com.TheDisappointedProgrammer.Drive
{
    public interface ILogger
    {
        bool LogLine(object message);
        bool Log(object o);
    }
}