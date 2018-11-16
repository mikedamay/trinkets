using System;
using Microsoft.EntityFrameworkCore;
using Nute.Entities;
using Version = Nute.Entities.Version;

namespace Nute
{
    public class NutritionDbContext : DbContext
    {
        // needed for design time
        public NutritionDbContext()
        {
            this.ChangeTracker.AutoDetectChangesEnabled = false;
        }
        
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
            CreateVersion(mb);
            CreateUser(mb);
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
                .Property(typeof(Int64), "Id");
            mb.Entity<Unit>()
                .Property(typeof(string), "Name");
            mb.Entity<Unit>()
                .Property(typeof(string), "Abbrev");
            mb.Entity<Unit>()
                .HasKey("Id");
            mb.Entity<Unit>()
                .HasAlternateKey("Name")
                .HasName("AK_Unit_Name");
            mb.Entity<Unit>()
                .HasAlternateKey("Abbrev")
                .HasName("AK_Unit_Abbrev");
           
            mb.Entity<Unit>().HasData(
                new {Id = 1L, Name = "gram", Abbrev = "g"}
                ,new {Id = 2L, Name = "each", Abbrev = "ea"}
                ,new {Id = 3L, Name = "large", Abbrev = "lge"}
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
            mb.Entity<NutrientProfile>()
                .HasOne(np => np.Version)
                .WithOne(v => v.NutrientProfile)
//                .HasForeignKey("VersionId")
                ;
        }
        private void CreateVersion(ModelBuilder mb)
        {
            mb.Entity<Version>()
                .HasData(
                    new Version(DateTime.Today, null, 1L)
                );
        }

        private void CreateUser(ModelBuilder mb)
        {
            mb.Entity<User>()
                .HasData(new User(id: 1, token: "magic1")
                );
        }

        public DbSet<Nutrient> Nutrient { get; set; }
        public DbSet<NutrientProfile> NutrientProfile { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Version> Version { get; set; }
        public DbSet<User> User { get; set; }
    }
}