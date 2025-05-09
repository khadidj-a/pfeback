
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

        [ForeignKey("Unite")]
        public int idunite { get; set; } // Unité à laquelle on prête

        public int duree { get; set; }

        public DateTime datepret { get; set; }

        public int? iduniteemt { get; set; } // Unité émettrice

        public Equipement? Equipement { get; set; }
    }
}


