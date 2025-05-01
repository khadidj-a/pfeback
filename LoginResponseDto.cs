namespace PFE_PROJECT.DTOs
{
    public class LoginResponseDto
    {
        public int IdUser { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public string Libelle { get; set; }
        public int Idrole { get; set; }
    }
}
