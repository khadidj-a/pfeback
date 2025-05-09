using System.Collections.Generic;
using System.Threading.Tasks;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface IEquipementService
    {
        Task<IEnumerable<EquipementDTO>> GetAllAsync(EquipementFilterDTO? filter = null);
        Task<EquipementDTO?> GetByIdAsync(int id);
        Task<EquipementDTO?> GetByCodeAsync(string code);
        Task<EquipementDTO> CreateAsync(CreateEquipementDTO dto);
        Task<EquipementDTO?> UpdateAsync(int id, UpdateEquipementDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ValidateEtatAsync(string? état);
    }
} 