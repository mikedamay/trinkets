using Microsoft.EntityFrameworkCore;

namespace Nute
{
    public class NutritionDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            ob.UseSqlServer(
                "Server=localhost,1401;Database=nutrition;User Id=sa;Password=M1cromus");
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Nutrient>().HasData(
                new Nutrient {Id = 1, Name = "Energy"}
                , new Nutrient {Id = 2, Name = "Fat"}
                , new Nutrient {Id = 3, Name = "Saturated Fat"}
                , new Nutrient {Id = 4, Name = "Carbohydrate"}
                , new Nutrient {Id = 5, Name = "Sugars"}
                , new Nutrient {Id = 6, Name = "Fibre"}
                , new Nutrient {Id = 7, Name = "Protein"}
                , new Nutrient {Id = 8, Name = "Salt"}
            );
        }
        public DbSet<Nutrient> Nutrients { get; set; }
    }
}