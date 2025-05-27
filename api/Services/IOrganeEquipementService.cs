using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface IOrganeEquipementService
    {
        Task<IEnumerable<OrganeEquipementDTO>> GetByEquipementIdAsync(int ideqpt);
        Task<IEnumerable<OrganeEquipementDTO>> GetByTypeAndMarqueAsync(int typeId, int marqueId);
        Task<IEnumerable<OrganeEquipementDTO>> CreateAsync(CreateOrganeEquipementDTO dto);
        Task<OrganeEquipementDTO?> UpdateAsync(int ideqpt, int idorg, UpdateOrganeEquipementDTO dto);
        
        // New methods
        Task<OrganeEquipementDTO?> AddOrganeAsync(AddOrganeEquipementDTO dto);
        Task<bool> DeleteOrganeAsync(DeleteOrganeEquipementDTO dto);
        Task<OrganeEquipementDTO?> ModifyOrganeAsync(ModifyOrganeEquipementDTO dto);
    }
} 