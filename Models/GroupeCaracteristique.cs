using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    [Table("groupecaracteristique")]
    public class GroupeCaracteristique
    {
        public int idcarac { get; set; }
        public Caracteristique Caracteristique { get; set; } = null!;

        public int idgrpidq { get; set; }
        public GroupeIdentique GroupeIdentique { get; set; } = null!;
    }
}


