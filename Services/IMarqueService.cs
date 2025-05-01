using System.Collections.Generic;
using System.Threading.Tasks;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface IMarqueService
    {
        Task<IEnumerable<MarqueDTO>> GetAllMarquesAsync(  string? searchTerm = null,
    string? sortBy = null,
    bool ascending = true);
        Task<MarqueDTO?> GetMarqueByIdAsync(int id);
        Task<MarqueDTO> CreateMarqueAsync(CreateMarqueDTO marqueDto);
        Task<MarqueDTO?> UpdateMarqueAsync(int id, UpdateMarqueDTO marqueDto);
        Task<bool> DeleteMarqueAsync(int id);
        Task<bool> CanDeleteMarqueAsync(int id);
    }
}

