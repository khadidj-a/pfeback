using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idrole { get; set; }
        
        [Required]
        [StringLength(255)]
        public string libelle { get; set; }
        
        // Navigation property
        public virtual ICollection<Utilisateur> Utilisateurs { get; set; }
    }
}