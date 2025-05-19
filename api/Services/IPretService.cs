using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface IPretService
    {
        Task<IEnumerable<Pret>> GetAllAsync(string? search = null, string? sortBy = null, string? order = "asc", int? idUnite = null);
        Task<Pret> CreateAsync(CreatePretDTO dto, string role);
        Task<Pret?> GetByIdAsync(int id);
        Task<int> GetPretCountAsync();
        
        Task<string?> GetEtatEquipementAsync(int idEquipement);
        Task<IEnumerable<PretDTO>> GetByUniteAsync(int idUnite, string? search = null, string? sortBy = null, string? order = "asc");
Task MettreAJourPositionPhysiqueAsync();
    }
}