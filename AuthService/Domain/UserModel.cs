using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain
{
    public class UserModel
    {
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Username { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
