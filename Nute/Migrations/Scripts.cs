namespace Nute.Migrations
{
public partial class NutritionProfileView
{
private string upScript = @"
create view NutrientProfile_v
  as
    select NutrientProfile.Id
        ,NutrientProfile.Name
        , concat(_dailyRecommendedMaxCount
            , Unit.Abbrev) as [Daily Recommended Amount]
        , Version.SequenceNumber as [Version]
        , Active
    from NutrientProfile
           inner join Unit
             on Unit.Id = _dailyRecommendedMaxUnitId
           inner join Version
             on Version.Id = VersionId
";
}
}