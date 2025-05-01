using System.ComponentModel.DataAnnotations;

namespace PFE_PROJECT.Models
{
    public class OrganeEquipementDTO
    {
        public int idorg { get; set; }
        public int ideqpt { get; set; }
        public string numsérie { get; set; } = string.Empty;
        public string nomOrgane { get; set; } = string.Empty;
    }

    public class CreateOrganeEquipementDTO
    {
        [Required]
        public int ideqpt { get; set; }
        
        [Required]
        public List<OrganeInfoDTO> Organes { get; set; } = new();
    }

    public class OrganeInfoDTO
    {
        [Required]
        public int idorg { get; set; }
        
        [Required]
        [StringLength(100)]
        public string numsérie { get; set; } = string.Empty;
    }
} 