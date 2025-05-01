using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    public class TypeEquip
    {
        [Key]
        public int idtypequip { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string? codetype { get; set; } // Généré automatiquement
        
        [Required]
        [StringLength(255)]
        public string designation { get; set; } = string.Empty;
    }
}
