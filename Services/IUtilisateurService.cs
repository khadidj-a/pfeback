using System.Collections.Generic;
using System.Threading.Tasks;
using PFE_PROJECT.Models;
using PFE_PROJECT.DTOs;

namespace PFE_PROJECT.Services
{
    public interface IUtilisateurService
    {
        Utilisateur AuthenticateAdmin(string email, string motpasse);
        Task<List<Utilisateur>> GetAllAsync();
        Task<Utilisateur> GetByIdAsync(int id);
        Task<Utilisateur> CreateAsync(Utilisateur utilisateur);
        Task<Utilisateur> UpdateAsync(Utilisateur utilisateur);
        Task<Utilisateur> ActivateDeactivateAsync(int id, bool activate);

        // ✅ NEW METHODS FOR EXTENDED LOGIN FUNCTIONALITY
        Task<Utilisateur> GetByEmailAsync(string email);
        Task<string> GetRoleNameByIdAsync(int idRole);
    }
}
