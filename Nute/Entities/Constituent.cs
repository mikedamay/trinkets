using System.ComponentModel.DataAnnotations.Schema;

namespace Nute.Entities
{
    public class Constituent
    {
        public Constituent()
        {
            
        }
        public Constituent(Nutrient nutrient, Quantity quantity
          ,Quantity servingSize, long id = 0)
        {
            Nutrient = nutrient;
            Quantity = quantity;
            ServingSize = servingSize;
            Id = id;
        }
        public long Id { get; private set; }
        public Nutrient Nutrient { get; private set; }

        #region Quantity
        [NotMapped]
        public Quantity Quantity
        {
            get => new Quantity(count: _quantityCount, unit:  _quantityUnit);
            set
            {
                _quantityCount = value.Count;
                _quantityUnit = value.Unit;
            }
        }
        private decimal _quantityCount;
#pragma warning disable 169
        private long _quantityUnitId;
#pragma warning restore 169
        private Unit _quantityUnit;
        #endregion

        #region ServingSize`
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
        #endregion
    }
}