using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nute.Entities
{
    public class NutrientProfile
    {
        public NutrientProfile()
        {
            
        }

        public NutrientProfile(Nutrient nutrient, Quantity servingSize
            , Quantity dailyRecommendedMax, Version version, long id = 0)
        {
            Id = id;
            Nutrient = nutrient;
            Version = version;
            NutrientId = nutrient.Id;
            ServingSize = servingSize;
            DailyRecommendedMax = dailyRecommendedMax;
        }
        public long Id { get; private set; }
        [Required]
        public Nutrient Nutrient { get; private set; }
        [Required]
        public long NutrientId { get; private set; }
        
        public Version Version { get; private set; }
        public long VersionId { get; private set; }

        [NotMapped]
        public Quantity ServingSize
        {
            get => new Quantity(count: _servingSizeCount, unit:  _servingSizeUnit);
            set
            {
                _servingSizeCount = value.Count;
                _servingSizeUnit = value.Unit;
            }
        }
        private decimal _servingSizeCount;
#pragma warning disable 169
        private long _servingSizeUnitId;
#pragma warning restore 169
        private Unit _servingSizeUnit;

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
}