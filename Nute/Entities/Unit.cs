using System.ComponentModel.DataAnnotations;

namespace Nute.Entities
{
    public class Unit
    {
        public Unit()
        {
            
        }
        public Unit(string name, string abbrev, long id = 0)
        {
            Id = id;
            Name = name;
            Abbrev = abbrev;
        }
        public long Id { get; private set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; private set; }
        [Required]
        [MaxLength(5)]
        public string Abbrev { get; private set; }
    }
}