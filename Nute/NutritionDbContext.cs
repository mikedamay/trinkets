using Microsoft.EntityFrameworkCore;
using Nute.Entities;

namespace Nute
{
    public class NutritionDbContext : DbContext
    {
        // needed for design time
        public NutritionDbContext()
        {
            this.ChangeTracker.AutoDetectChangesEnabled = false;            
        }
        
        // needed to build
/*
        public NutritionDbContext(DbContextOptions<NutritionDbContext> opts) : base(opts)
        {
        }
*/
        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            ob.UseSqlServer(
                "Server=localhost,1401;Database=nutrition;User Id=sa;Password=M1cromus");
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Nutrient>()
                .HasAlternateKey(n => n.Name)
                .HasName("AK_Nutrient_Name");
            mb.Entity<Nutrient>().HasData(
                new Nutrient (id : 1, name : "Energy")
                , new Nutrient (id : 2, name : "Fat")
                , new Nutrient (id : 3, name : "Saturated Fat")
                , new Nutrient (id : 4, name : "Carbohydrate")
                , new Nutrient (id : 5, name : "Sugars")
                , new Nutrient (id : 6, name : "Fibre")
                , new Nutrient (id : 7, name : "Protein")
                , new Nutrient (id : 8, name : "Salt")
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
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
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