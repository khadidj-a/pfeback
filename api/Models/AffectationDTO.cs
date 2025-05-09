using System.ComponentModel.DataAnnotations;

namespace PFE_PROJECT.Models
{
    public class AffectationDTO
    {
        public int idaffec { get; set; }
        public int ideqpt { get; set; }
        public int idunite { get; set; }
        public DateTime dateaffec { get; set; }
        public string? designationEquipement { get; set; }
        public string? designationUnite { get; set; }
    }

    public class CreateAffectationDTO
    {
        [Required(ErrorMessage = "L'ID de l'équipement est requis")]
        public int ideqpt { get; set; }

        [Required(ErrorMessage = "L'ID de l'unité est requis")]
        public int idunite { get; set; }

        [Required(ErrorMessage = "La date d'affectation est requise")]
        public DateTime dateaffec { get; set; }
    }
} 