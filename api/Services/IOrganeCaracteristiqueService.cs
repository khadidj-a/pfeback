using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface IOrganeCaracteristiqueService
    {
        Task<IEnumerable<OrganeCaracteristiqueDTO>> GetByOrganeIdAsync(int id_organe);
        Task<IEnumerable<OrganeCaracteristiqueDTO>> CreateAsync(CreateOrganeCaracteristiqueDTO dto);
        Task<OrganeCaracteristiqueDTO?> UpdateAsync(int id_organe, int id_caracteristique, UpdateOrganeCaracteristiqueDTO dto);
    }
} 