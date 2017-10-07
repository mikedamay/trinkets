using System;

namespace ting
{
    class Program
    {
		static Reader rdr = new Reader();
        static void Main(string[] args)
        {
			//foreach(var line in rdr.Read<string>(st => st))
			//{
			//	Console.WriteLine(line);
			//}
			RunGetter();
        }
		static void RunGetter()
		{
			Getter getter = new Getter();
			getter.Get();
		}
    }
}
