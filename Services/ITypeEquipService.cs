using System.Collections.Generic;
using System.Threading.Tasks;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface ITypeEquipService
    {
        Task<IEnumerable<TypeEquipDTO>> GetAllTypeEquipsAsync(string? searchTerm = null, string? sortBy = null, bool ascending = true);
        Task<TypeEquipDTO?> GetTypeEquipByIdAsync(int id);
        Task<TypeEquipDTO> CreateTypeEquipAsync(CreateTypeEquipDTO typeEquipDto);
        Task<TypeEquipDTO?> UpdateTypeEquipAsync(int id, UpdateTypeEquipDTO typeEquipDto);
        Task<bool> DeleteTypeEquipAsync(int id);
        Task<bool> CanDeleteTypeEquipAsync(int id);
    }
}
