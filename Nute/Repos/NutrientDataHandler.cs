using System;
using System.Linq;
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
        public NutrientProfile GetNutrientProfile(long id)
        {
            return dbContext.NutrientProfile.FirstOrDefault(np => np.Id == id);
        }

        public void SaveNutrientProfile(NutrientProfile np)
        {
            int versionSequenceNumber;
            var aaa = new [] {new Version(0, DateTime.MinValue)}.Concat(dbContext.Version
              .Where(v => v.NutrientProfile.Nutrient.Id == np.Nutrient.Id));
/*
            var bbb = 
              aaa.DefaultIfEmpty(new Version(0, DateTime.MinValue));
              // incurred run-time error - DefaultIfEmpty() was expecting 'int'
*/
            versionSequenceNumber = aaa.Max(v => v.SequenceNumber);
            var latest = new Version(versionSequenceNumber + 1, DateTime.Today);
            dbContext.Version.Add(latest);
            var np2 = new NutrientProfile(np.Nutrient, np.BodyType, np.DailyRecommendedMax, latest);
            dbContext.NutrientProfile.Add(np2);
            dbContext.SaveChanges();
        }
    }
}