using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    public class Categorie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idcategorie { get; set; }

        [Required]
        [StringLength(255)]
        public string designation { get; set; } = string.Empty;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string codecategorie { get; set; } = string.Empty;
    }
}

