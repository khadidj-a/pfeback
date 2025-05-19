using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PFE_PROJECT.Models
{
    public class OrganeEquipement
    {
        [ForeignKey("Organe")]
        public int idorg { get; set; }
        public Organe Organe { get; set; } = null!;

        [ForeignKey("Equipement")]
        public int ideqpt { get; set; }
        public Equipement Equipement { get; set; } = null!;

        [Required]
        [StringLength(100)]
        [JsonPropertyName("numserie")]
        public string numsérie { get; set; } = string.Empty;
    }
}

