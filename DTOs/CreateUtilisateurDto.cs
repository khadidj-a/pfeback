using System.ComponentModel.DataAnnotations;

namespace PFE_PROJECT.DTOs
{
    public class CreateUtilisateurDto
    {
        [Required]
        public int IdUnite { get; set; }
        
        [Required]
        [StringLength(255)]
        public required string Nom { get; set; }
        
        [Required]
        [StringLength(255)]
        public required string Prenom { get; set; }
        
        [StringLength(20)]
        public string? NumTel { get; set; } // Nullable si ce n'est pas une donnée obligatoire
        
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public required string Email { get; set; }
        
        [Required]
        [StringLength(255)]
        [MinLength(6)]
        public required string MotPasse { get; set; }
        
        [Required]
        public int IdRole { get; set; }
    }
}
