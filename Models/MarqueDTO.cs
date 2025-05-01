using System;
using System.ComponentModel.DataAnnotations;

namespace PFE_PROJECT.Models
{
    public class MarqueDTO
    {
        public int idmarque { get; set; }
        public string codemarque { get; set; } = string.Empty;
        [Required]
        public string nom_fabriquant { get; set; } = string.Empty;
    }
    
    public class CreateMarqueDTO
    {   [Required]
        public string nom_fabriquant { get; set; } = string.Empty;
    }
    
    public class UpdateMarqueDTO
    {   [Required]
        public string nom_fabriquant { get; set; } = string.Empty;
    }
}



