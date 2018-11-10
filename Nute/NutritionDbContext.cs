using Microsoft.EntityFrameworkCore;
using Nute.Entities;

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
            mb.Entity<Unit>().HasData(
                new Unit{Id = 1, Name="Gram", Abbrev = "g"}
                , new Unit{Id = 2, Name="Each", Abbrev = "ea"}
                , new Unit{Id = 3, Name="Large", Abbrev = "lge"}
            );
            mb.Entity<NutrientProfile>()
                .HasOne<Nutrient>()
                .WithMany()
                .HasForeignKey("NutrientId")
                .OnDelete(DeleteBehavior.Restrict);
            mb.Entity<NutrientProfile>()
                .Property("_servingSizeCount");
            mb.Entity<NutrientProfile>()
                .Property("_servingSizeUnitId");
            mb.Entity<NutrientProfile>()
                .HasOne<Unit>("_servingSizeUnit")
                .WithMany()
                .HasForeignKey("_servingSizeUnitId")
                .OnDelete(DeleteBehavior.Restrict);
            mb.Entity<NutrientProfile>()
                .Property("_dailyRecommendedAmountCount");
            mb.Entity<NutrientProfile>()
                .Property("_dailyRecommendedAmountUnitId");
            mb.Entity<NutrientProfile>()
                .HasOne<Unit>("_dailyRecommendedAmountUnit")
                .WithMany()
                .HasForeignKey("_dailyRecommendedAmountUnitId")
                .OnDelete(DeleteBehavior.Restrict);

        }
        public DbSet<Nutrient> Nutrient { get; set; }
        public DbSet<NutrientProfile> NutrientProfile { get; set; }
        public DbSet<Unit> Unit { get; set; }
    }
}