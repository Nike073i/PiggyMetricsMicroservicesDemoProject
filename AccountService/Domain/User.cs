using System;
using System.ComponentModel.DataAnnotations;

namespace AccountService.Domain
{
    public class User
    {
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Username { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
