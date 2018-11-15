using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nute.Migrations
{
    public partial class start : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nutrient",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nutrient", x => x.Id);
                    table.UniqueConstraint("AK_Nutrient_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Abbrev = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Id);
                    table.UniqueConstraint("AK_Unit_Abbrev", x => x.Abbrev);
                    table.UniqueConstraint("AK_Unit_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Token = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Version",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Version", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NutrientProfile",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NutrientId = table.Column<long>(nullable: false),
                    VersionId = table.Column<long>(nullable: false),
                    _dailyRecommendedMaxCount = table.Column<decimal>(nullable: false),
                    _dailyRecommendedMaxUnitId = table.Column<long>(nullable: false),
                    _servingSizeCount = table.Column<decimal>(nullable: false),
                    _servingSizeUnitId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutrientProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NutrientProfile_Nutrient_NutrientId",
                        column: x => x.NutrientId,
                        principalTable: "Nutrient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NutrientProfile_Version_VersionId",
                        column: x => x.VersionId,
                        principalTable: "Version",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NutrientProfile_Unit__dailyRecommendedMaxUnitId",
                        column: x => x._dailyRecommendedMaxUnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NutrientProfile_Unit__servingSizeUnitId",
                        column: x => x._servingSizeUnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Nutrient",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Energy" },
                    { 2L, "Fat" },
                    { 3L, "Saturated Fat" },
                    { 4L, "Carbohydrate" },
                    { 5L, "Sugars" },
                    { 6L, "Fibre" },
                    { 7L, "Protein" },
                    { 8L, "Salt" }
                });

            migrationBuilder.InsertData(
                table: "Unit",
                columns: new[] { "Id", "Abbrev", "Name" },
                values: new object[,]
                {
                    { 1L, "g", "Gram" },
                    { 2L, "ea", "Each" },
                    { 3L, "lge", "Large" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Token" },
                values: new object[] { 1L, "magic1" });

            migrationBuilder.InsertData(
                table: "Version",
                columns: new[] { "Id", "EndDate", "StartDate" },
                values: new object[] { 1L, null, new DateTime(2018, 11, 15, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.CreateIndex(
                name: "IX_NutrientProfile_NutrientId",
                table: "NutrientProfile",
                column: "NutrientId");

            migrationBuilder.CreateIndex(
                name: "IX_NutrientProfile_VersionId",
                table: "NutrientProfile",
                column: "VersionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NutrientProfile__dailyRecommendedMaxUnitId",
                table: "NutrientProfile",
                column: "_dailyRecommendedMaxUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_NutrientProfile__servingSizeUnitId",
                table: "NutrientProfile",
                column: "_servingSizeUnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NutrientProfile");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Nutrient");

            migrationBuilder.DropTable(
                name: "Version");

            migrationBuilder.DropTable(
                name: "Unit");
        }
    }
}
