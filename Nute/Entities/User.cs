using System.ComponentModel.DataAnnotations;

namespace Nute.Entities
{
    public class User
    {
        public long Id { get; private set; }
        [Required]
        [MaxLength(64)]
        public string Token { get; private set; }

        public User()
        {
            
        }
        public User(int id = 0, string token = "")
        {
            Id = id;
            Token = token;
        }
    }
}