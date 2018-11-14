using System.ComponentModel.DataAnnotations;

namespace Nute.Entities
{
    public class Luper
    {
        public int Id { get; set; }
        [Required]
        public Lud Lud { get; set; }
    }
}