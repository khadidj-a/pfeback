namespace PFE_PROJECT.Models
{
    public class CreateGroupeIdentiqueDTO
    {
        public int id_marque { get; set; }
        public int id_type_equip { get; set; }
        public List<int> id_organes { get; set; } = new();
        public List<int> id_caracteristiques { get; set; } = new();
    }
} 