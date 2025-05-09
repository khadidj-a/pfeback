using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface IAffectationService
    {
        Task<IEnumerable<AffectationDTO>> GetAllAsync(string? searchTerm = null, string? sortBy = null, bool ascending = true);
        Task<AffectationDTO> CreateAsync(CreateAffectationDTO dto);
        Task<AffectationDTO?> GetByIdAsync(int id);
    }
} 