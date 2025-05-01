using PFE_PROJECT.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PFE_PROJECT.Services
{
    public interface IUniteService
    {
        Task<IEnumerable<UniteDTO>> GetAllUnitesAsync(string? searchTerm = null, string? sortBy = null, bool ascending = true);
        Task<UniteDTO?> GetUniteByIdAsync(int id);
        Task<UniteDTO> CreateUniteAsync(CreateUniteDTO uniteDto);
        Task<UniteDTO?> UpdateUniteAsync(int id, UpdateUniteDTO uniteDto);
        Task<bool> CanDeleteUniteAsync(int id);
        Task<bool> DeleteUniteAsync(int id);
        Task<IEnumerable<Wilaya>> GetAllWilayasAsync();
        Task<IEnumerable<Region>> GetAllRegionsAsync();
    }
}
