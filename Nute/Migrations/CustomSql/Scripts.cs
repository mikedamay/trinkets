namespace Nute.Migrations.CustomSql
{
    public static partial class SundryViews
    {
        private static string upScript = @"
create view Nutrient_v
  as
    select Nutrient.Id
        , Nutrient.Name as Nutrient
    from Nutrient
go
create view BodyType_v
  as
    select BodyType.Id
        , BodyType.Name
    from BodyType
go
            ";

        private static string downScript = @"
drop view Nutrient_v
go
drop view BodyType_v
go
";
    }
}