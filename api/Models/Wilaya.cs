using System.ComponentModel.DataAnnotations;

namespace PFE_PROJECT.Models
{
    public class Wilaya
    {
        [Key]
        public int idwilaya { get; set; }

        [Required]
        [StringLength(100)]
        public string nomwilaya { get; set; } = string.Empty;
    }
}
