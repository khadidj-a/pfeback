using System;

namespace PFE_PROJECT.Models
{
    public class ReformeDTO
    {
        public int idref { get; set; }
        public int ideqpt { get; set; }
        public string ?motifref { get; set; } = string.Empty;
        public DateTime dateref { get; set; }
        public int numdes { get; set; }
    }

public class CreateReformeDTO
{
   public int ideqpt { get; set; }
   public string ?motifref { get; set; }
   public DateTime dateref { get; set; }
   public int numdes { get; set; }
}

}



