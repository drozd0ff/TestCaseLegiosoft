using System.ComponentModel.DataAnnotations;

namespace TestCaseLegiosoft.Models
{
    public class UserModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
