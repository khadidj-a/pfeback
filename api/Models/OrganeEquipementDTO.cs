using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace PFE_PROJECT.Models
{
    public class OrganeEquipementDTO
    {
        public int idorg { get; set; }
        public int ideqpt { get; set; }
        [JsonPropertyName("numserie")]
        public string numsérie { get; set; } = string.Empty;
        public string nomOrgane { get; set; } = string.Empty;
    }

    public class CreateOrganeEquipementDTO
    {
        [Required]
        public int ideqpt { get; set; }
        
        [Required]
        public List<OrganeValeurDTO> Organes { get; set; } = new();
    }

    public class OrganeValeurDTO
    {
        [Required]
        public int idorg { get; set; }
        
        [Required]
        [StringLength(255)]
        [JsonPropertyName("numserie")]
        public string numsérie { get; set; } = string.Empty;
    }

    public class UpdateOrganeEquipementDTO
    {
        [Required]
        [StringLength(255)]
        public string numsérie { get; set; } = string.Empty;
    }

    public class AddOrganeEquipementDTO
    {
        [Required]
        public int ideqpt { get; set; }
        
        [Required]
        public int idorg { get; set; }
        
        [Required]
        [StringLength(255)]
        public string numsérie { get; set; } = string.Empty;
    }

    public class DeleteOrganeEquipementDTO
    {
        [Required]
        public int ideqpt { get; set; }
        
        [Required]
        public int idorg { get; set; }
    }

    public class ModifyOrganeEquipementDTO
    {
        [Required]
        public int ideqpt { get; set; }
        
        [Required]
        public int idorg { get; set; }
        
        [Required]
        [StringLength(255)]
        public string numsérie { get; set; } = string.Empty;
    }
} 