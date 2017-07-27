
namespace com.TheDisappointedProgrammer.Drive
{
    class Program
    {
        static void Main(string[] args)
        {
            IOCC.Instance.GetOrCreateObjectTree<SheetProcessor>(typeof(SheetProcessor)).Process();
        }
    }
}
