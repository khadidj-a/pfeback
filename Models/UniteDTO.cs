namespace PFE_PROJECT.Models
{
    public class UniteDTO
    {
        public int idunite { get; set; }
        public string codeunite { get; set; } = string.Empty;
        public string designation { get; set; } = string.Empty;
        public int idwilaya { get; set; }
        public string nomwilaya { get; set; } = string.Empty;
        public int idregion { get; set; }
        public string nomregion { get; set; } = string.Empty;
    }

    public class CreateUniteDTO
    {
        public string designation { get; set; } = string.Empty;
        public int idwilaya { get; set; }
        public int idregion { get; set; }
    }

    public class UpdateUniteDTO
    {
        public string designation { get; set; } = string.Empty;
        public int idwilaya { get; set; }
        public int idregion { get; set; }
    }
}
