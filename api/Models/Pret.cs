
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    public class Pret
    {
        [Key]
        public int idpret { get; set; }

        [ForeignKey("Equipement")]
        public int ideqpt { get; set; }
        public Equipement? Equipement { get; set; }

        [ForeignKey("UniteDestination")]
        public int idunite { get; set; } // Unité à laquelle on prête
        public Unite? UniteDestination { get; set; }

        [ForeignKey("UniteEmettrice")]
        public int? iduniteemt { get; set; } // Unité émettrice
        public Unite? UniteEmettrice { get; set; }

        public int duree { get; set; }

        public DateTime datepret { get; set; }
        public string? motif { get; set; } // nouveau champ

    }
}
