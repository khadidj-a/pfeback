using System.ComponentModel.DataAnnotations;

namespace PFE_PROJECT.Models
{
    public class OrganeCaracteristiqueDTO
    {
        public int id_organe { get; set; }
        public int id_caracteristique { get; set; }
        public string valeur { get; set; } = string.Empty;
        public string nomCaracteristique { get; set; } = string.Empty;
    }

    public class CreateOrganeCaracteristiqueDTO
    {
        [Required]
        public int id_organe { get; set; }
        
        [Required]
        public List<CaracteristiqueValeurDTO> Caracteristiques { get; set; } = new();
    }

    public class UpdateOrganeCaracteristiqueDTO
    {
        [Required]
        [StringLength(255)]
        public string valeur { get; set; } = string.Empty;
    }
}
