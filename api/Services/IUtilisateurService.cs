
using System.Collections.Generic;
using System.Threading.Tasks;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface IUtilisateurService
    {
        Task<Utilisateur?> AuthenticateAdminAsync(string email, string motpasse); // Renamed to async
        Task<List<Utilisateur>> GetAllAsync();
        Task<Utilisateur> GetByIdAsync(int id);
        Task<Utilisateur> CreateAsync(Utilisateur utilisateur);
        Task<Utilisateur> UpdateAsync(Utilisateur utilisateur);
        Task<Utilisateur> ActivateDeactivateAsync(int id, bool activate);

        // Extended login/auth support
        Task<Utilisateur?> GetByEmailAsync(string email);
        Task<string?> GetRoleNameByIdAsync(int idRole);
        Task<Utilisateur?> VerifyUserActiveStatus(string email);
        Task<bool> EmailExistsAsync(string email);
         bool VerifyPassword(string hashedPassword, string providedPassword);
         
        Task<int> GetUtilisateurCountAsync();
    }
}