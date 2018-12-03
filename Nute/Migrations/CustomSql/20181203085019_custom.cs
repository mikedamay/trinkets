using Microsoft.EntityFrameworkCore.Migrations;

namespace Nute.Migrations.CustomSql
{
    public partial class custom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            SundryViews.Up(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            SundryViews.Down(migrationBuilder);
        }
    }
}
