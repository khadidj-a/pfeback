using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PFE_PROJECT.Models
{
    public class Utilisateur
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-génération de l'ID
        public int iduser { get; set; }
        
        [Required]
        public int idunite { get; set; }
        
        [Required]
        [StringLength(255)]
        public string nom { get; set; }
        
        [Required]
        [StringLength(255)]
        public string prenom { get; set; }
        
        [StringLength(20)]
        public string numtel { get; set; }
        
        [Required]
        [StringLength(255)]
        public string email { get; set; }
        
        [Required]
        [StringLength(255)]
        public string motpasse { get; set; }
        
        [Required]
        public int idrole { get; set; }
        
        // Navigation property - mais on l'ignore dans la sérialisation JSON
        [JsonIgnore]
        public virtual Role Role { get; set; }
        
        // Using a boolean to represent account activation status
        public string Actif { get; set; }  // Default value is 'true' (active)
    }
}