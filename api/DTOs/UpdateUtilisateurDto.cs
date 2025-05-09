using System.ComponentModel.DataAnnotations;

namespace PFE_PROJECT.DTOs
{
    public class UpdateUtilisateurDto
    {
        public int? IdUnite { get; set; }
        
        [StringLength(255)]
        public string Nom { get; set; }
        
        [StringLength(255)]
        public string Prenom { get; set; }
        
        [StringLength(20)]
        public string NumTel { get; set; }
        
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }
        
        [StringLength(255)]
        [MinLength(6)]
        public string MotPasse { get; set; }
        
        // Renommé pour correspondre à ce que le modèle binding attend
        public int? IdRole { get; set; }
    }
}