using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface IGroupeOrganeService
    {
        Task<IEnumerable<GroupeOrganeDTO>> GetAllAsync();
        Task<IEnumerable<GroupeOrganeDTO>> GetByGroupeIdAsync(int idgrpidq);
        Task<IEnumerable<GroupeOrganeDTO>> CreateAsync(int idgrpidq, List<int> idorgs);
        Task<bool> DeleteByGroupeIdAsync(int idgrpidq);
    }
} 