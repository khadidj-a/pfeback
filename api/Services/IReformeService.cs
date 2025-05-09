using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface IReformeService
    {
        Task<IEnumerable<ReformeDTO>> GetAllAsync(string? search = null, string? sortBy = null, string? order = "asc");
        Task<ReformeDTO?> GetByIdAsync(int id);
        Task<IEnumerable<Reforme>> GetByUniteAsync(int idUnite, string? search, string? sortBy, string? order);
 Task<ReformeDTO> CreateAsync(CreateReformeDTO dto);
    }
}


