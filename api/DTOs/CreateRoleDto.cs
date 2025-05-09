using System.ComponentModel.DataAnnotations;

namespace PFE_PROJECT.DTOs
{
    public class CreateRoleDto
    {
        [Required]
        [StringLength(255)]
        public string Libelle { get; set; } = string.Empty; // Initialise à une valeur par défaut non-nullable
    }
}

