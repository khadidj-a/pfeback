using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    [Table("organecaracteristique")]
    public class OrganeCaracteristique
    {
        public int id_organe { get; set; }
        public Organe Organe { get; set; } = null!;

        public int id_caracteristique { get; set; }
        public Caracteristique Caracteristique { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string valeur { get; set; } = string.Empty;
    }
}

