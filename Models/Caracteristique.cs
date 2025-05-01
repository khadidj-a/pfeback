using System.ComponentModel.DataAnnotations;

namespace PFE_PROJECT.Models
{
    public class Caracteristique
    {
        [Key]
        public int id_caracteristique { get; set; }

        [Required]
        [StringLength(255)]
        public string libelle { get; set; } = string.Empty;
    }
}