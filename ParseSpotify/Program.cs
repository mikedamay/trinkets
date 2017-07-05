using System;
using System.IO;

namespace ParseSpotify
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var fs = File.Open("mike.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(line);
                        }

                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                
            }
            Console.WriteLine("Hello World!");
        }
    }
}
