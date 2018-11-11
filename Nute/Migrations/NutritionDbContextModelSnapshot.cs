﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nute;

namespace Nute.Migrations
{
    [DbContext(typeof(NutritionDbContext))]
    partial class NutritionDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Nute.Entities.Nutrient", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasAlternateKey("Name")
                        .HasName("AK_Nutrient_Name");

                    b.ToTable("Nutrient");

                    b.HasData(
                        new { Id = 1L, Name = "Energy" },
                        new { Id = 2L, Name = "Fat" },
                        new { Id = 3L, Name = "Saturated Fat" },
                        new { Id = 4L, Name = "Carbohydrate" },
                        new { Id = 5L, Name = "Sugars" },
                        new { Id = 6L, Name = "Fibre" },
                        new { Id = 7L, Name = "Protein" },
                        new { Id = 8L, Name = "Salt" }
                    );
                });

            modelBuilder.Entity("Nute.Entities.NutrientProfile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("NutrientId");

                    b.Property<long?>("NutrientId1");

                    b.Property<decimal>("_dailyRecommendedAmountCount");

                    b.Property<long>("_dailyRecommendedAmountUnitId");

                    b.Property<decimal>("_servingSizeCount");

                    b.Property<long>("_servingSizeUnitId");

                    b.HasKey("Id");

                    b.HasIndex("NutrientId");

                    b.HasIndex("NutrientId1");

                    b.HasIndex("_dailyRecommendedAmountUnitId");

                    b.HasIndex("_servingSizeUnitId");

                    b.ToTable("NutrientProfile");
                });

            modelBuilder.Entity("Nute.Entities.Unit", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Abbrev")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Unit");

                    b.HasData(
                        new { Id = 1L, Abbrev = "g", Name = "Gram" },
                        new { Id = 2L, Abbrev = "ea", Name = "Each" },
                        new { Id = 3L, Abbrev = "lge", Name = "Large" }
                    );
                });

            modelBuilder.Entity("Nute.Entities.NutrientProfile", b =>
                {
                    b.HasOne("Nute.Entities.Nutrient")
                        .WithMany()
                        .HasForeignKey("NutrientId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Nute.Entities.Nutrient", "Nutrient")
                        .WithMany()
                        .HasForeignKey("NutrientId1");

                    b.HasOne("Nute.Entities.Unit", "_dailyRecommendedAmountUnit")
                        .WithMany()
                        .HasForeignKey("_dailyRecommendedAmountUnitId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Nute.Entities.Unit", "_servingSizeUnit")
                        .WithMany()
                        .HasForeignKey("_servingSizeUnitId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
