using System.ComponentModel.DataAnnotations;

namespace Nute.Entities
{
    public class Unit
    {
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