﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Nute.Migrations
{
    public partial class nutrientdata2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Nutrients",
                columns: new[] { "Id", "Name" },
                values: new object[] { 7L, "Fat" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Nutrients",
                keyColumn: "Id",
                keyValue: 7L);
        }
    }
}
