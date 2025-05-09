using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    public class Reforme
    {
        [Key]
        public int idref { get; set; }

        [ForeignKey("Equipement")]
        public int ideqpt { get; set; }

        public string motifref { get; set; } = string.Empty;

        public DateTime dateref { get; set; }

        public string numdes { get; set; } = string.Empty;
    }
}




