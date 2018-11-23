using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Nute.Entities
{
    public class Ingredient
    {
        public Ingredient(string name, IEnumerable<Constituent> constituents = null
            , long id = 0)
        {
            Name = name;
            if (constituents != null) Constituents = constituents;
            Id = id;
        }
        public long Id { get; private set; }
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