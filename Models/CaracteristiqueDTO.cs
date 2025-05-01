namespace PFE_PROJECT.Models
{
    public class CaracteristiqueDTO
    {
        public int id_caracteristique { get; set; }
        public string libelle { get; set; } = string.Empty;
    }

    public class CreateCaracteristiqueDTO
    {
        public string libelle { get; set; } = string.Empty;
    }

    public class UpdateCaracteristiqueDTO
    {
        public string libelle { get; set; } = string.Empty;
    }
}
