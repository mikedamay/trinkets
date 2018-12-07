using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Nute.Entities
{
    public class Unit
    {
        public const string KCAL = "kcal";
        public const string GRAM = "g";
        public const string EACH = "each";
        public const string LGE = "lge";
        public const string MILLIGRAM = "mg";
        public const string MICROGRAM = "\u03bcg";
        public Unit()
        {
            
        }
        public Unit(string name, string abbrev)
        {
            Name = name;
            Abbrev = abbrev;
        }
        [Required]
        [MaxLength(50)]
        public string Name { get; private set; }
        [Required]
        [MaxLength(5)]
        public string Abbrev { get; private set; }
    }
}