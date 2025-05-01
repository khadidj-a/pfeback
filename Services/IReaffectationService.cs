using PFE_PROJECT.Models;
namespace PFE_PROJECT.Services
{
    public interface IReaffectationService
    {
        Task<IEnumerable<ReaffectationDTO>> GetAllAsync(string? search = null, string? sortBy = null, string? order = "asc");
        Task<ReaffectationDTO> CreateAsync(CreateReaffectationDTO dto);
        Task<ReaffectationDTO?> GetByIdAsync(int id);
        Task<IEnumerable<ReaffectationDTO>> GetByUniteAsync(int idUnite, string? search = null, string? sortBy = null, string? order = "asc");


    }
}
