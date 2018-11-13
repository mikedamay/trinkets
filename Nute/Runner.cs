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

        [Fact]
        public void Write_NutrientProfile()
        {
            Unit grams = dbContext.Unit.FirstOrDefault(u => u.Name == "Gram");
            if (grams == null)
            {
                throw new Exception("The database has been corrupted - there is no unit Gram available");
            }

            var fat = dbContext.Nutrient.FirstOrDefault(n => n.Name == "Fat");
            if (fat == null)
            {
                throw new Exception("The database has been corrupted - there is no nutrient Fat available");
            }
            var np = new NutrientProfile(
                nutrient: fat
                , servingSize: new Quantity(100, grams)
                , dailyRecommendedAount: new Quantity(1000, grams)
                );
            dbContext.NutrientProfile.Add(np);
            dbContext.SaveChanges();
            dbContext.Database.CommitTransaction();
        }
    }
}