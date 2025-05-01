using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    public class Unite
    {
        [Key]
        public int idunite { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string? codeunite { get; set; }

        [Required]
        [StringLength(255)]
        public string designation { get; set; } = string.Empty;

        [ForeignKey("Wilaya")]
public int idwilaya { get; set; }
public Wilaya Wilaya { get; set; } = null!;

[ForeignKey("Region")]
public int idregion { get; set; }
public Region Region { get; set; } = null!;

    }
}
