using System.ComponentModel.DataAnnotations;

namespace Nute.Entities
{
    public class Nutrient
    {
        public long Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}