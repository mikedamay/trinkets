using Microsoft.EntityFrameworkCore.Migrations;

/*
    Migrations Workflow (incl. Custom Migrations)
    ---------------------------------------------
 
    Constraints & Dependencies:
    a) We git commit NutritionDbContextModelSnapshot which is required to support Database.Migrate()
    b) We git ignore model based migrations (in NutritionDbContext) so as not to confuse other devs.
    c) Database is disposable - completely re-constructable
    d) Model based migrations should not have dependencies on custom migrations.

    From scratch (from clean install of code-base):
    1. drop all objects from database
    2. remove any migrations generated from model (i.e. any migrations that are direct
       children of the Migrations directory) that happen to be hanging about.  Do not
       remove any files in the Migrations/CustomSql directory.
    3. remove NutritionDbContextModelSnapshot (because this is  a book mark
       for the current state of of migrations generated from model - we need that to be null)
    4. execute "dotnet ef migrations add --context NutritionDbContext start" 
       - to generate migrations from model
    5. execute "dotnet ef database update --context NutritionDbContext"
     
    Incremental (how to handle model changes on dev's machine as code evolves):
    1. Add a static class containing the Up() and Down() in CustomSql. 
    2. execute "dotnet ef migrations add --context NutritionDbContext <something>" - to generate migrations from model
    5. execute "dotnet ef database update --context NutritionDbContext"
    i.e you can simply operate on the nutrition db context using defaults  Commands
    like "migrations remove" and "database update 0" should work well.
    
    Changes to custom scripts:
    1. execute "dotnet ef database update --context CustomSqlContext 0"
        check: views created by the script should no longer be present in the Nutrition database 
    2. Make changes to migrations/CustomSql/Scripts.cs. to add or remove views
    3. execute "dotnet ef database update --context CustomSqlContext"
        check: views created by the script should now be present in the Nutrition database 
*/

namespace Nute.Migrations.CustomSql
{
    public static partial class SundryViews
    {
        internal static void Up(MigrationBuilder mib)
        {
            mib.Sql(upScript);
        }

        internal static void Down(MigrationBuilder mib)
        {
            mib.Sql(downScript);
        }
    }
}
