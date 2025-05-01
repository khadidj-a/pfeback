using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    public class CaracteristiqueEquipement
    {
        [ForeignKey("Caracteristique")]
        public int idcarac { get; set; }
        public Caracteristique Caracteristique { get; set; } = null!;

        [ForeignKey("Equipement")]
        public int ideqpt { get; set; }
        public Equipement Equipement { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string valeur { get; set; } = string.Empty;
    }
}
