using System.ComponentModel.DataAnnotations;

namespace Nute.Entities
{
    public class BodyType
    {
        public long Id { get; private set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; private set; }
        [Required]
        [MaxLength(50)]
        public string Description { get; private set; }
        public BodyType(string name, string description, long id = 0)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}