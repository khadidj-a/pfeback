using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    public class Affectation
    {
        [Key]
        public int idaffec { get; set; }

        [ForeignKey("Equipement")]
        public int ideqpt { get; set; }
        public Equipement Equipement { get; set; } = null!;

        [ForeignKey("Unite")]
        public int idunite { get; set; }
        public Unite Unite { get; set; } = null!;

        [Required]
        public DateTime dateaffec { get; set; }
    }
}
