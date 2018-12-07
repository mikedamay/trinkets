using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nute.Entities;
using Nute.Repos;
using Xunit;
using Version = Nute.Entities.Version;

namespace Nute
{
    public class Runner
    {
        private readonly NutritionDbContext dbContext;

        public Runner()
        {
            dbContext = new NutritionDbContext();
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
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
        public void Write_Ingredient()
        {
            var ndh = new NutrientDataHandler(dbContext);
            var nutrients = ndh.LoadNutrients();
            var units = ndh.LoadUnits();
            var branFlakes = new Ingredient(
                name: "Bran Flakes"
                , servingSize: new Quantity(125, units[Unit.GRAM])
                ,constituents: new List<Constituent>
                {
                    new Constituent(
                      nutrient: nutrients[TestConstants.Energy]
                      ,quantity: new Quantity(354, units[Unit.KCAL])
                      ,servingSize: new Quantity(100, units[Unit.GRAM]) )                                            
                }
            );
            ndh.SaveIngredient(branFlakes);
            dbContext.Database.CommitTransaction();
        }

        [Fact]
        public void Delete_Ingredient()
        {
            var ndh = new NutrientDataHandler(dbContext);
            var ingredient = ndh.GetIngredient(3);
            
        }
/*
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
            var bodyType = dbContext.BodyType.FirstOrDefault(n => n.Name == "Male");
            if (bodyType == null)
            {
                throw new Exception("The database has been corrupted - there is no body type Male available");
            }
            var np = new NutrientProfile(
                nutrient: fat, bodyType: bodyType, dailyRecommendedMax: new Quantity(1000, grams)
                );
            var ndh = new NutrientDataHandler(dbContext);
            ndh.SaveNutrientProfile(np);
            var list = dbContext.NutrientProfile.ToList();
            dbContext.Database.CommitTransaction();
        }
*/
    }

    public static class TestConstants
    {
        public const string Energy = "Energy";
    }
}