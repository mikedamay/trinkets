using System;
using Microsoft.EntityFrameworkCore;
using Nute.Common;
using Nute.Entities;
using Version = Nute.Entities.Version;


// https://www.nutritionix.com/business/api
// https://www.gov.uk/government/publications/composition-of-foods-integrated-dataset-cofid

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
            CreateBodyType(mb);
            CreateNutrient(mb);
            CreateUnit(mb);
            CreateVersion(mb);
            CreateUser(mb);
            CreateConstituent(mb);
            CreateIngredient(mb);
        }

        private void CreateIngredient(ModelBuilder mb)
        {
            mb.Entity<Ingredient>()
                .Property("_servingSizeCount")
                .HasColumnName("servingSizeCount");
            mb.Entity<Ingredient>()
                .HasOne<Unit>("_servingSizeUnit")
                .WithMany();
            mb.Entity<Ingredient>()
                .HasAlternateKey(i => i.Name)
                .HasName("AK_Ingredient_Name")
                  ;
            mb.Entity<Ingredient>()
                .Property("_servingSizeUnitId")
                .HasColumnName("ServingSizeUnitId");
            mb.Entity<Ingredient>()
                .HasMany(ii => ii.Constituents)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void CreateConstituent(ModelBuilder mb)
        {
            mb.Entity<Constituent>()
                .Property("_quantityCount")
                .HasColumnName("QuantityCount");
            mb.Entity<Constituent>()
                .Property("_quantityUnitId")
                .HasColumnName("QuantityUnitId");
            mb.Entity<Constituent>()
                .HasOne<Unit>("_quantityUnit")
                .WithMany()
                // specifying an FK here will create a shadow
                // property and make that the FK rather than
                // using QuantityUnitId
                // ditto for ServingSize and Ingredient.ServingSize
                .OnDelete(DeleteBehavior.ClientSetNull);
            mb.Entity<Constituent>()
                .Property("_servingSizeCount")
                .HasColumnName("servingSizeCount");
            mb.Entity<Constituent>()
                .Property("_servingSizeUnitId")
                .HasColumnName("ServingSizeUnitId");
            mb.Entity<Constituent>()
                .HasOne<Unit>("_servingSizeUnit")
                .WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull);
        }

        private void CreateBodyType(ModelBuilder mb)
        {
            mb.Entity<BodyType>()
                .HasData(
                    new BodyType("Male", "Male Std.", 1L)
                    ,new BodyType("Female", "Female Std.", 2L)
                );
        }

        private void CreateNutrient(ModelBuilder mb)
        {
            mb.Entity<Nutrient>()
                .HasAlternateKey(n => n.Name)
                .HasName("AK_Nutrient_Name");
            mb.Entity<Nutrient>().HasData(
                new Nutrient (id : 1, name : "Energy")
                , new Nutrient (id : 2, name : "Fat")
                , new Nutrient (id : 3, name : "Saturated Fat", subsidiary: true)
                , new Nutrient (id : 4, name : "Carbohydrate")
                , new Nutrient (id : 5, name : "Sugars", subsidiary: true)
                , new Nutrient (id : 6, name : "Fibre")
                , new Nutrient (id : 7, name : "Protein")
                , new Nutrient (id : 8, name : "Salt")
                , new Nutrient (id : 9, name : "Thiamin (B1)")
                , new Nutrient (id : 10, name : "Riboflavin (B2)")
                , new Nutrient (id : 11, name : "Niacin")
                , new Nutrient (id : 12, name : "Vitamin B6")
                , new Nutrient (id : 13, name : "Folic Acid (B9)")
                , new Nutrient (id : 14, name : "Vitamin B12")
                , new Nutrient (id : 15, name : "Iron")
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
                ,new {Id = 4L, Name = "calorie", Abbrev = "kcal"}
                ,new {Id = 5L, Name = "milligram", Abbrev = "mg"}
                ,new {Id = 6L, Name = "micro-gram", Abbrev = "\u03bcg"}
            );
        }

        private void CreateVersion(ModelBuilder mb)
        {
            mb.Entity<Version>()
                .HasData(
                    new Version(1, DateTime.Today, null, 1L)
                );
        }

        private void CreateUser(ModelBuilder mb)
        {
            mb.Entity<User>()
                .HasAlternateKey(Constants.Token);
            mb.Entity<User>()
                .HasData(new User(id: 1, token: "magic1"))
                ;
        }

        public DbSet<Nutrient> Nutrient { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Version> Version { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<BodyType> BodyType { get; set; }
        public DbSet<Constituent> Constituent { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
    }
}