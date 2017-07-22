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
                int rowNum = 1;
                foreach (var fileName in args)
                {
                    using (var fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (var sr = new StreamReader(fs))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                var parts = line.Split('\u2013');
                                Console.WriteLine($"{rowNum++}\t{Path.GetFileNameWithoutExtension(fileName)}\t{parts[0].Trim()}\t{parts[1].Trim()}");
                            }
                        }
                    }

                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                
            }
        }
        private static void AnalyzeLine(string line)
        {
            for (int ii = 0; ii < line.Length; ii++)
            {
                Console.WriteLine($"{line.Substring(ii, 1)} {Char.ConvertToUtf32(line, ii)}");
            }
        }
    }
}
