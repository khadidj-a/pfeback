using System.ComponentModel.DataAnnotations;

namespace PFE_PROJECT.Models
{
    public class CaracteristiqueEquipementDTO
    {
        public int ideqpt { get; set; }
        public int idcarac { get; set; }
        public string designation { get; set; } = string.Empty;
        public string valeur { get; set; } = string.Empty;
        public string nomcarac { get; set; } = string.Empty;
    }

    public class CreateCaracteristiqueEquipementDTO
    {
        [Required]
        public int ideqpt { get; set; }
        
        [Required]
        public int idcarac { get; set; }
        
        [Required]
        [StringLength(255)]
        public string valeur { get; set; } = string.Empty;
    }

    public class UpdateCaracteristiqueEquipementDTO
    {
        [Required]
        [StringLength(255)]
        public string valeur { get; set; } = string.Empty;
    }

    public class BulkCreateCaracteristiqueEquipementDTO
    {
        [Required]
        public int ideqpt { get; set; }
        
        [Required]
        public List<CaracteristiqueValeurDTO> Caracteristiques { get; set; } = new();
    }

    public class CaracteristiqueValeurDTO
    {
        [Required]
        public int idcarac { get; set; }
        
        [Required]
        [StringLength(255)]
        public string valeur { get; set; } = string.Empty;
    }
} 