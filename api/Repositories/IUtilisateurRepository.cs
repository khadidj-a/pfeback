using PFE_PROJECT.Models;

namespace PFE_PROJECT.Repositories
{
    public interface IUtilisateurRepository
    {
        Utilisateur? GetAdminUserByEmailAndPassword(string email, string motpasse);

        Task<List<Utilisateur>> GetAllAsync();
        Task<Utilisateur?> GetByIdAsync(int id);
        Task<Utilisateur> CreateAsync(Utilisateur utilisateur);
        Task<Utilisateur> UpdateAsync(Utilisateur utilisateur);
        // Task DeleteAsync(int id);
        Task<Utilisateur?> ActivateDeactivateAsync(int id, bool activate);
        Task<Utilisateur> GetByEmailAsync(string email);

    }
}
