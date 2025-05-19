using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    public class Reaffectation
    {
        [Key]
        public int idreaf { get; set; }

        // 🔧 Clé étrangère vers Equipement
        [ForeignKey("Equipement")]
        public int ideqpt { get; set; }

        // 🔧 Clé étrangère vers l'unité d’émission
        [ForeignKey("UniteEmission")]
        public int? iduniteemt { get; set; }

        // 🔧 Clé étrangère vers l'unité de destination
        [ForeignKey("UniteDestination")]
        public int idunitedest { get; set; }

        public DateTime datereaf { get; set; }

        public string motifreaf { get; set; } = string.Empty;

        // 🧭 Propriétés de navigation
        public Equipement? Equipement { get; set; }
        public Unite? UniteEmission { get; set; }
        public Unite? UniteDestination { get; set; }
    }
}
