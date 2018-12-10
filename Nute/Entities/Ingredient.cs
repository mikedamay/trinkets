using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Nute.Entities
{
    public class Ingredient
    {
        public Ingredient()
        {
            
        }
        public Ingredient(string shortCode, string name
            , IEnumerable<Constituent> constituents = null
            , Quantity servingSize = default(Quantity)
            , long id = 0)
        {
            ShortCode = shortCode;
            Name = name;
            if (constituents != null) Constituents = constituents;
            if (servingSize == default(Quantity))
            {
                ServingSize = new Quantity(100, new Unit("gram", "gram"));
            }
            else
            {
                ServingSize = servingSize;
            }
            Id = id;
        }
        public long Id { get; private set; }
        [Required]
        [MaxLength(10)]
        public string ShortCode { get; private set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; private set; }
        public IEnumerable<Constituent> Constituents { get; private set; } 
          = new List<Constituent>();
        
        #region ServingSize
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