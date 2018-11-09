using Microsoft.EntityFrameworkCore.Migrations;

namespace Nute.Migrations
{
    public partial class nutrientdata3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Nutrients",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Name",
                value: "Protein");

            migrationBuilder.InsertData(
                table: "Nutrients",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 2L, "Fat" },
                    { 3L, "Saturated Fat" },
                    { 4L, "Carbohydrate" },
                    { 5L, "Sugars" },
                    { 6L, "Fibre" },
                    { 8L, "Salt" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Nutrients",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Nutrients",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Nutrients",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Nutrients",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Nutrients",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Nutrients",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.UpdateData(
                table: "Nutrients",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Name",
                value: "Fat");
        }
    }
}
