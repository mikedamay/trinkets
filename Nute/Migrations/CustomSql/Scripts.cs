namespace Nute.Migrations.CustomSql
{
    public static partial class NutritionProfileView
    {
        private static string upScript = @"
create view NutrientProfile_v
  as
    select NutrientProfile.Id
        , Nutrient.Name as Nutrient
        , BodyType.Name as [Body Type]
        , concat(DailyRecommendedMaxCount
            , Unit.Abbrev) as [Daily Recommended Amount]
        , Version.SequenceNumber as [Version]
        , Active
    from NutrientProfile
           inner join Unit
             on Unit.Id = DailyRecommendedMaxUnitId
           inner join Version
             on Version.Id = VersionId
           inner join Nutrient
             on Nutrient.Id = NutrientId
           inner join BodyType
             on BodyType.Id = BodyTypeId
            ";
    }
}