using System.ComponentModel.DataAnnotations;

namespace PFE_PROJECT.Models
{
    public class Region
    {
        [Key]
        public int idregion { get; set; }

        [Required]
        [StringLength(100)]
        public string nomregion { get; set; } = string.Empty;
    }
}
