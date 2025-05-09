using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    [Table("groupe_identique")]
    public class GroupeIdentique
    {
        [Key]
        [Column("id_groupe_identique")]
        public int Id { get; set; }

        [Column("codegrp")]
        public string codegrp { get; set; } = string.Empty;

        [ForeignKey("Marque")]
        [Column("id_marque")]
        public int id_marque { get; set; }
        public Marque Marque { get; set; } = null!;

        [ForeignKey("TypeEquip")]
        [Column("id_type_equip")]
        public int id_type_equip { get; set; }
        public TypeEquip TypeEquip { get; set; } = null!;

        public ICollection<GroupeOrgane> GroupeOrganes { get; set; } = new List<GroupeOrgane>();
        public ICollection<GroupeCaracteristique> GroupeCaracteristiques { get; set; } = new List<GroupeCaracteristique>();
    }
}