using System;

namespace PFE_PROJECT.Models
{
    public class ReformeDTO
    {
        public int idref { get; set; }
        public int ideqpt { get; set; }
        public string motifref { get; set; } = string.Empty;
        public DateTime dateref { get; set; }
        public string numdes { get; set; } = string.Empty;
    }

    public class CreateReformeDTO
    {
        public int idEquipement { get; set; }
        public string motif { get; set; } = string.Empty;
        public DateTime dateReforme { get; set; }
        public string numeroDecision { get; set; } = string.Empty;
    }
}



