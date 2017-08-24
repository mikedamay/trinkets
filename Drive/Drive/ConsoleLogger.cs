using System;
using com.TheDisappointedProgrammer.IOCC;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean]
    public class ConsoleLogger : ILogger
    {
        public bool LogLine(object message)
        {
            Console.WriteLine(message);
            return true;
        }

        public bool Log(object o)
        {
            Console.Write(o);
            return true;
        }
    }
}