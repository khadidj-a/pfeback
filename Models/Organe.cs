using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PFE_PROJECT.Models; // à ajouter

public class Organe
{
    [Key]
    public int id_organe { get; set; }

    public string code_organe { get; set; } = string.Empty;
    public string libelle_organe { get; set; } = string.Empty;
    public string modele { get; set; } = string.Empty;

    [ForeignKey("Marque")]
    public int id_marque { get; set; }
    public Marque Marque { get; set; } = null!;

    public ICollection<OrganeCaracteristique> OrganeCaracteristiques { get; set; } = new List<OrganeCaracteristique>();
}
