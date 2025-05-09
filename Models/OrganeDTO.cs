using System.ComponentModel.DataAnnotations;
using PFE_PROJECT.DTOs;
namespace PFE_PROJECT.Models
{
    public class OrganeDTO
{
    public int id_organe { get; set; }
    public string code_organe { get; set; } = string.Empty;
    public string libelle_organe { get; set; } = string.Empty;
    public string modele { get; set; } = string.Empty;
    public int id_marque { get; set; }
    public string nom_marque { get; set; } = string.Empty;

    public List<OrganeCaracteristiqueDTO> caracteristiques { get; set; } = new();
}


   public class CreateOrganeDTO
{
    [Required]
    public string libelle_organe { get; set; } = string.Empty;
    
    [Required]
    public string modele { get; set; } = string.Empty;
    
    [Required]
    public int id_marque { get; set; }
    
    public List<OrganeCaracteristiqueCreateDTO> caracteristiques { get; set; } = new();
}

public class UpdateOrganeDTO
{
    [Required]
    public string libelle_organe { get; set; } = string.Empty;
    
    [Required]
    public string modele { get; set; } = string.Empty;
    
    [Required]
    public int id_marque { get; set; }
    
    public List<OrganeCaracteristiqueCreateDTO> caracteristiques { get; set; } = new();
}

// Nouveau DTO intermédiaire pour la création/mise à jour :
public class OrganeCaracteristiqueCreateDTO
{
    public int id_caracteristique { get; set; }
    [StringLength(255)]
    public string valeur { get; set; } = string.Empty;
}


    
}
