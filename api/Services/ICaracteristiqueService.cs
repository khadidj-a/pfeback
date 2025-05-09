using System.Collections.Generic;
using System.Threading.Tasks;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface ICaracteristiqueService
    {
        Task<IEnumerable<CaracteristiqueDTO>> GetAllCaracteristiquesAsync(string? searchTerm = null, string? sortBy = null, bool ascending = true);
        Task<CaracteristiqueDTO?> GetCaracteristiqueByIdAsync(int id);
        Task<CaracteristiqueDTO> CreateCaracteristiqueAsync(CreateCaracteristiqueDTO dto);
        Task<CaracteristiqueDTO?> UpdateCaracteristiqueAsync(int id, UpdateCaracteristiqueDTO dto);
        Task<bool> CanDeleteCaracteristiqueAsync(int id);
        Task<bool> DeleteCaracteristiqueAsync(int id);
        Task<IEnumerable<CaracteristiqueDTO>> GetByTypeAndMarqueAsync(int typeId, int marqueId);
        
        Task<int> GetCaracteristiqueCountAsync();
    }
}