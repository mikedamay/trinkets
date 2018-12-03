using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nute.Entities
{
/*
    public class NutrientProfile
    {
        public NutrientProfile()
        {
            
        }

        public NutrientProfile(Nutrient nutrient
            , BodyType bodyType
            , Quantity dailyRecommendedMax, Version version = null, long id = 0)
        {
            Id = id;
            Nutrient = nutrient;
            BodyType = bodyType;
            Version = version;
            DailyRecommendedMax = dailyRecommendedMax;
        }
        public long Id { get; private set; }
        [Required]
        // e.g. "Fibre"
        public Nutrient Nutrient { get; private set; }
        [Required]
        public BodyType BodyType { get; private set; }
        
        public Version Version { get; private set; }
        public long VersionId { get; private set; }

        [NotMapped]
        public Quantity DailyRecommendedMax
        {
            get => new Quantity(count: _dailyRecommendedMaxCount, unit:  _dailyRecommendedMaxUnit);
            set
            {
                _dailyRecommendedMaxCount = value.Count;
                _dailyRecommendedMaxUnit = value.Unit;
            }
        }
        private decimal _dailyRecommendedMaxCount;
#pragma warning disable 169
        private long _dailyRecommendedMaxUnitId;
#pragma warning restore 169
        private Unit _dailyRecommendedMaxUnit;
    }
*/
}