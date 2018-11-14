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
            CreateNutrient(mb);
            CreateUnit(mb);
            CreateNutrientProfile(mb);
        }

        private void CreateNutrient(ModelBuilder mb)
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
        }

        private void CreateUnit(ModelBuilder mb)
        {
            mb.Entity<Unit>()
                .HasAlternateKey(u => u.Name)
                .HasName("AK_Unit_Name");
            mb.Entity<Unit>()
                .HasAlternateKey(u => u.Abbrev)
                .HasName("AK_Unit_Abbrev");
           
            mb.Entity<Unit>().HasData(
                new Unit(id : 1, name:"Gram", abbrev : "g")
                , new Unit(id : 2, name:"Each", abbrev : "ea")
                , new Unit(id : 3, name:"Large", abbrev : "lge")
            );
        }

        private void CreateNutrientProfile(ModelBuilder mb)
        {
            mb.Entity<NutrientProfile>()
                .HasOne(n => n.Nutrient)
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
                .Property("_dailyRecommendedMaxCount");
            mb.Entity<NutrientProfile>()
                .Property("_dailyRecommendedMaxUnitId");
            mb.Entity<NutrientProfile>()
                .HasOne<Unit>("_dailyRecommendedMaxUnit")
                .WithMany()
                .HasForeignKey("_dailyRecommendedMaxUnitId")
                .OnDelete(DeleteBehavior.Restrict);

/*
            mb.Entity<NutrientProfile>()
                .Property("_dailyRecommendedMinCount");
            mb.Entity<NutrientProfile>()
                .Property("_dailyRecommendedMinUnitId");
            mb.Entity<NutrientProfile>()
                .HasOne<Unit>("_dailyRecommendedMinUnit")
                .WithMany()
                .HasForeignKey("_dailyRecommendedMinUnitId")
                .OnDelete(DeleteBehavior.Restrict);
*/
        }
        public DbSet<Nutrient> Nutrient { get; set; }
        public DbSet<NutrientProfile> NutrientProfile { get; set; }
        public DbSet<Unit> Unit { get; set; }
    }
}