using System;
using System.ComponentModel.DataAnnotations;
namespace PFE_PROJECT.Models
{
  public class GroupeIdentiqueDTO
{
    public int Id { get; set; }
    public string CodeGrp { get; set; } = string.Empty;
    public string MarqueNom { get; set; } = string.Empty;
    public string TypeEquipNom { get; set; } = string.Empty;

    public int IdType { get; set; }     
    public int IdMarque { get; set; }    

    public List<string> Organes { get; set; } = new();
    public List<int> OrganesIds { get; set; } = new(); // 👈 ajouté
    public List<string> Caracteristiques { get; set; } = new();
    public List<int> CaracteristiquesIds { get; set; } = new(); // 👈 ajouté
}

}
