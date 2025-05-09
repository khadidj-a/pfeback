using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    public class Marque
    {
        [Key]
        public int idmarque { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string? codemarque { get; set; } // Marqué comme nullable avec ?
        
        [Required]
        [StringLength(255)]
        public string nom_fabriquant { get; set; } = string.Empty;
    }
}