namespace PFE_PROJECT.Models
{
    public class ReaffectationDTO
    {
        public int idreaf { get; set; }
        public int ideqpt { get; set; }
        public int? iduniteemt { get; set; }
        public int idunitedest { get; set; }
        public DateTime datereaf { get; set; }
        public string motifreaf { get; set; } = string.Empty;
    }

    public class CreateReaffectationDTO
    {
        public int idEquipement { get; set; }
        public int idUniteDestination { get; set; }
        public DateTime date { get; set; }
        public string motif { get; set; } = string.Empty;
    }
}
