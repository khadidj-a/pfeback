using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    [Table("groupeorgane")]
    public class GroupeOrgane
    {
        public int idorg { get; set; }
        public Organe Organe { get; set; } = null!;

        public int idgrpidq { get; set; }
        public GroupeIdentique GroupeIdentique { get; set; } = null!;
    }
}

