using System;

namespace CoreSQLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new mikeContext())
            {
                foreach (var name in db.Names)
                {
                    Console.WriteLine(name.FirstName);
                    
                }
            }
            Console.ReadKey();
                Console.WriteLine("Hello World!");
        }
    }
}