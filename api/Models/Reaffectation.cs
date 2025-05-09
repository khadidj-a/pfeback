using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    public class Reaffectation
    {
        [Key]
        public int idreaf { get; set; }

        [ForeignKey("Equipement")]
        public int ideqpt { get; set; }

        public int? iduniteemt { get; set; }

        [ForeignKey("Unite")]
        public int idunitedest { get; set; }

        public DateTime datereaf { get; set; }

        public string motifreaf { get; set; } = string.Empty;

        public Equipement? Equipement { get; set; }
    }
}
