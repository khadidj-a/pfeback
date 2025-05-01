using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface IGroupeCaracteristiqueService
    {
        Task<IEnumerable<GroupeCaracteristiqueDTO>> GetAllAsync();
        Task<IEnumerable<GroupeCaracteristiqueDTO>> GetByGroupeIdAsync(int idgrpidq);
        Task<IEnumerable<GroupeCaracteristiqueDTO>> CreateAsync(int idgrpidq, List<int> idcaracs);
        Task<bool> DeleteByGroupeIdAsync(int idgrpidq);
    }
} 