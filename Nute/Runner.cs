using System;
using System.Linq;
using Nute.Entities;
using Xunit;

namespace Nute
{
    public class Runner
    {
        private readonly NutritionDbContext dbContext;

        public Runner()
        {
            dbContext = new NutritionDbContext();
            dbContext.Database.BeginTransaction();
        }

        [Fact]
        public void Write_Nutrient()
        {
            dbContext.Nutrient.Add(new Nutrient(id: 0, name: "Test Nutrient"));
            dbContext.SaveChanges();
            Assert.NotNull(dbContext.Nutrient.ToList().FirstOrDefault(n => n.Name == "Test Nutrient"));
        }
    }
}