using System;
using System.ComponentModel.DataAnnotations;
namespace PFE_PROJECT.Models
{
    public class CategorieDTO
    {
        public int idcategorie { get; set; }
        public string designation { get; set; } = string.Empty;
        public string codecategorie { get; set; } = string.Empty;
    }

    public class CreateCategorieDTO
    {
        [Required]
        public string designation { get; set; } = string.Empty;
    }

    public class UpdateCategorieDTO
    {
        [Required]
        public string designation { get; set; } = string.Empty;
    }
}


