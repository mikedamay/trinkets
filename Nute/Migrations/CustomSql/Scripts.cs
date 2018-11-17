namespace Nute.Migrations.CustomSql
{
    public static partial class NutritionProfileView
    {
        private static string upScript = @"
            create view NutrientProfile_v
              as
                select NutrientProfile.Id
                    ,NutrientProfile.Name
                    , concat(DailyRecommendedMaxCount
                        , Unit.Abbrev) as [Daily Recommended Amount]
                    , Version.SequenceNumber as [Version]
                    , Active
                from NutrientProfile
                       inner join Unit
                         on Unit.Id = DailyRecommendedMaxUnitId
                       inner join Version
                         on Version.Id = VersionId
            ";
    }
}