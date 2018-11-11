using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nute.Entities
{
    public class NutrientProfile
    {
        public long Id { get; set; }
        public Nutrient Nutrient { get; set; }
        [Required]
        public long NutrientId { get; set; }

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
        public Quantity DailyRecommendedAmount
        {
            get => new Quantity(count: _dailyRecommendedAmountCount, unit:  _dailyRecommendedAmountUnit);
            set
            {
                _dailyRecommendedAmountCount = value.Count;
                _dailyRecommendedAmountUnit = value.Unit;
            }
        }
        private decimal _dailyRecommendedAmountCount;
#pragma warning disable 169
        private long _dailyRecommendedAmountUnitId;
#pragma warning restore 169
        private Unit _dailyRecommendedAmountUnit;
    }
}