using System.Collections.Generic;
using System.Threading.Tasks;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface IOrganeService
    {
        Task<IEnumerable<OrganeDTO>> GetAllAsync(string? searchTerm = null,
    string? sortBy = null,
    bool ascending = true);
        Task<OrganeDTO?> GetByIdAsync(int id);
        Task<OrganeDTO> CreateAsync(CreateOrganeDTO dto);
        Task<OrganeDTO?> UpdateAsync(int id, UpdateOrganeDTO dto);
        Task<bool> CanDeleteAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
