using System;
using System.ComponentModel.DataAnnotations;
namespace PFE_PROJECT.Models
{
    public class CategorieDTO
    {
        public int idcategorie { get; set; }

        [Required]  // Ajoute cette annotation pour la validation
        public string categorie_principale { get; set; } = string.Empty;
        
        public string codecategorie { get; set; } = string.Empty;

        [Required]
        public string designation { get; set; } = string.Empty;
    }

    public class CreateCategorieDTO
    {
        [Required]
        public string categorie_principale { get; set; } = string.Empty;

        [Required]
        public string designation { get; set; } = string.Empty;
    }

    public class UpdateCategorieDTO
    {
        [Required]
        public string designation { get; set; } = string.Empty;
    }
}


