using System;
using System.ComponentModel.DataAnnotations;
namespace PFE_PROJECT.Models
{
    public class TypeEquipDTO
    {
        public int idtypequip { get; set; }
        public string codetype { get; set; } = string.Empty;
        [Required]
        public string designation { get; set; } = string.Empty;
    }
    
    public class CreateTypeEquipDTO
    {   [Required]
        public string designation { get; set; } = string.Empty;
    }
    
    public class UpdateTypeEquipDTO
    {   [Required]
        public string designation { get; set; } = string.Empty;
    }
}
