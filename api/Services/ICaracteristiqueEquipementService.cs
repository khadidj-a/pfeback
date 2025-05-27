using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface ICaracteristiqueEquipementService
    {
        Task<IEnumerable<CaracteristiqueEquipementDTO>> GetByEquipementIdAsync(int ideqpt, bool showValue = true);
        Task<IEnumerable<CaracteristiqueEquipementDTO>> GetByTypeAndMarqueAsync(int typeId, int marqueId);
        Task<IEnumerable<CaracteristiqueEquipementDTO>> CreateAsync(BulkCreateCaracteristiqueEquipementDTO dto);
        Task<IEnumerable<CaracteristiqueEquipementDTO>> BulkCreateAsync(BulkCreateCaracteristiqueEquipementDTO dto);
        Task<CaracteristiqueEquipementDTO?> UpdateAsync(int ideqpt, int idcarac, UpdateCaracteristiqueEquipementDTO dto);
        
        // New methods
        Task<CaracteristiqueEquipementDTO?> AddCaracteristiqueAsync(AddCaracteristiqueEquipementDTO dto);
        Task<bool> DeleteCaracteristiqueAsync(DeleteCaracteristiqueEquipementDTO dto);
        Task<CaracteristiqueEquipementDTO?> ModifyCaracteristiqueAsync(ModifyCaracteristiqueEquipementDTO dto);
    }
} 