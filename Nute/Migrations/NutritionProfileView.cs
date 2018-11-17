using Microsoft.EntityFrameworkCore.Migrations;

namespace Nute.Migrations
{
    public partial class NutritionProfileView : Migration
    {
        protected override void Up(MigrationBuilder mib)
        {
            mib.Sql(upScript);
        }

        protected override void Down(MigrationBuilder mib)
        {
            mib.Sql("drop view NutrientProfile_v");
        }
    }
}
