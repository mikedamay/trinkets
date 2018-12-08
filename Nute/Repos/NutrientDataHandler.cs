using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nute.Entities;
using Version = Nute.Entities.Version;

namespace Nute.Repos
{
    internal class NutrientDataHandler
    {
        private NutritionDbContext dbContext;
        public NutrientDataHandler(NutritionDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
//        public NutrientProfile GetNutrientProfile(long id)
//        {
//            return dbConte//xt.NutrientProfile.FirstOrDefault(np => np.Id == id);
//        }

        public void SaveNutrientProfile(/*NutrientProfile np*/)
        {
//            int versionSequenceNumber;
/*
            var aaa = new [] {new Version(0, DateTime.MinValue)}.Concat(dbContext.Version
              .Where(v => v.NutrientProfile.Nutrient.Id == np.Nutrient.Id));
*/
/*
            var bbb = 
              aaa.DefaultIfEmpty(new Version(0, DateTime.MinValue));
              // incurred run-time error - DefaultIfEmpty() was expecting 'int'
*/
/*
            versionSequenceNumber = aaa.Max(v => v.SequenceNumber);
            var latest = new Version(versionSequenceNumber + 1, DateTime.Today);
            dbContext.Version.Add(latest);
            dbContext.SaveChanges();
*/
        }

        public IReadOnlyDictionary<string, Nutrient> LoadNutrients()
        {
            return dbContext.Nutrient.ToDictionary(n => n.Name, n => n);
        }
        public IReadOnlyDictionary<string, Unit> LoadUnits()
        {
            return dbContext.Unit.ToDictionary(u => u.Abbrev, u => u);
        }

        public void SaveIngredient(Ingredient ingredient)
        {
            foreach (var constituent in ingredient.Constituents)
            {
                dbContext.Constituent.Add(constituent);
            }

            dbContext.Ingredient.Add(ingredient);
            dbContext.SaveChanges();
        }

        public Ingredient GetIngredient(long id)
        {
            return dbContext.Ingredient
                .Include("Constituents.Nutrient")
                .FirstOrDefault(i => i.Id == id);
        }

        public void DeleteIngredient(Ingredient ingredient)
        {
            dbContext.Ingredient.Remove(ingredient);
            dbContext.SaveChanges();
        }
    }
}