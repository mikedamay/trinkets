using System.ComponentModel.DataAnnotations;
using System.Data;
// https://www.nrv.gov.au/resources/nrv-summary-tables

namespace Nute.Entities
{
    public class Nutrient
    {
        public Nutrient()
        {
            
        }
        public Nutrient( string shortCode
            , string name, bool subsidiary = false, int id = 0)
        {
            Id = id;
            ShortCode = shortCode;
            Name = name;
            Subsidiary = subsidiary;
        }
        public long Id { get; private set; }
        [Required]
        [MaxLength(10)]
        public string ShortCode { get; private set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; private set; }
        [Required]
        public bool Subsidiary { get; private set; }
    }
}