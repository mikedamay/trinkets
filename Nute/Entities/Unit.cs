using System.ComponentModel.DataAnnotations;

namespace Nute.Entities
{
    public class Unit
    {
        public long Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(5)]
        public string Abbrev { get; set; }
    }
}