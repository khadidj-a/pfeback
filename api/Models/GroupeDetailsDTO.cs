namespace PFE_PROJECT.Models
{
    public class GroupeDetailsDTO
    {
        public int Id { get; set; }
        public string CodeGrp { get; set; } = string.Empty;
        public string MarqueNom { get; set; } = string.Empty;
        public string TypeEquipNom { get; set; } = string.Empty;
        
        public List<OrganeDetailDTO> Organes { get; set; } = new();
        public List<CaracteristiqueDetailDTO> Caracteristiques { get; set; } = new();
    }

    public class OrganeDetailDTO
    {
        public int idorg { get; set; }
        public string libelle_organe { get; set; } = string.Empty;
    }

    public class CaracteristiqueDetailDTO
    {
        public int idcarac { get; set; }
        public string libelle { get; set; } = string.Empty;
    }
} 