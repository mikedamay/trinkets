using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nute.Entities;

namespace Nute
{
    class Program
    {
        static void Main(string[] args)
        {
            NutritionDbContext nc = new NutritionDbContext();
            var nut = new Nutrient(0, "another nut");
            nc.Nutrient.Add(nut);
            nc.SaveChanges();
            foreach (var nutter in nc.Nutrient.ToList())
            {
                Console.WriteLine(nutter.Name);
            }
        }
    }
}
