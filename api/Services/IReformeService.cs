using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface IReformeService
    {
        Task<IEnumerable<ReformeDTO>> GetAllAsync(string? search = null, string? sortBy = null, string? order = "asc");
        Task<ReformeDTO?> GetByIdAsync(int id);
        Task<IEnumerable<ReformeDTO>> GetByUniteAsync(int idUnite, string? search = null, string? sortBy = null, string? order = "asc");

 Task<ReformeDTO> CreateAsync(CreateReformeDTO dto);
 
        Task<int> GetReformeCountAsync();
        Task<string?> GetEtatEquipementAsync(int idEquipement);
        Task<bool> NumeroDecisionExistsAsync(string numeroDecision);


    }
}


