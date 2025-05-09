using System.ComponentModel.DataAnnotations;

namespace PFE_PROJECT.DTOs
{public class UserDto
{
    public int iduser{get; set; }

    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string NumTel { get; set; }
    public string Email { get; set; }
    public int IdUnite { get; set; }
    public bool Actif { get; set; }
    public int idrole { get; set; }

    public string motpasse {get; set; }
    
}

}

