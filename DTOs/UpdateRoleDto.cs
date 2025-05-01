using System.ComponentModel.DataAnnotations;

namespace PFE_PROJECT.DTOs
{
    public class UpdateRoleDto
    {
        [Required]
        [StringLength(255)]
        public string Libelle { get; set; }
    }
}

