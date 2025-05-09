using System.ComponentModel.DataAnnotations;

namespace PFE_PROJECT.DTOs
{
    public class RoleDto
    {
        public int IdRole { get; set; }

        [Required]
        [StringLength(255)]
        public required string Libelle { get; set; }
    }
}


