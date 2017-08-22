using System;
using com.TheDisappointedProgrammer.IOCC;


namespace com.TheDisappointedProgrammer.Drive
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SimpleIOCContainer sic = new SimpleIOCContainer();
                SheetProcessor sp = sic.CreateAndInjectDependencies<SheetProcessor>();
                sp.Process();
            }
            catch (IOCCException iex)
            {
                Console.WriteLine(iex.Diagnostics);
            }
        }

    }
}
