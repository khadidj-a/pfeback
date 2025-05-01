using System.ComponentModel.DataAnnotations;

namespace PFE_PROJECT.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string motpasse { get; set; }
    }
}
