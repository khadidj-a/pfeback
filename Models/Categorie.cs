using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    public class Categorie
    {
        [Key]
        public int idcategorie { get; set; } // Auto-incrémenté

        [Required]
        [StringLength(10)]
        public string categorie_principale { get; set; } = string.Empty; // Doit être 'roulants', 'fixes', 'soutien'

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string? codecategorie { get; set; } // Généré automatiquement par SQL Server

        [Required]
        [StringLength(255)]
        public string designation { get; set; } = string.Empty;
    }
}

