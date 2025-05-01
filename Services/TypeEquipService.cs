using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public class TypeEquipService : ITypeEquipService
    {
        private readonly ApplicationDbContext _context;

        public TypeEquipService(ApplicationDbContext context)
        {
            _context = context;
        }
public async Task<IEnumerable<TypeEquipDTO>> GetAllTypeEquipsAsync(string? searchTerm = null, string? sortBy = null, bool ascending = true)
{
    var query = _context.TypeEquips.AsQueryable();

    // 🔍 Filtrage
    if (!string.IsNullOrEmpty(searchTerm))
    {
        string lowerSearch = searchTerm.ToLower();
        query = query.Where(te =>
            te.designation.ToLower().Contains(lowerSearch) ||
            (te.codetype != null && te.codetype.ToLower().Contains(lowerSearch)));
    }

    // ⬆⬇ Tri
    sortBy ??= "idtypequip";

    query = sortBy.ToLower() switch
    {
        "designation" => ascending ? query.OrderBy(te => te.designation) : query.OrderByDescending(te => te.designation),
        "codetype" => ascending ? query.OrderBy(te => te.codetype) : query.OrderByDescending(te => te.codetype),
        _ => ascending ? query.OrderBy(te => te.idtypequip) : query.OrderByDescending(te => te.idtypequip)
    };

    // 🟢 Exécution + Mapping
    var types = await query.ToListAsync();

    return types.Select(te => new TypeEquipDTO
    {
        idtypequip = te.idtypequip,
        codetype = te.codetype ?? "",
        designation = te.designation
    });
}


        public async Task<TypeEquipDTO?> GetTypeEquipByIdAsync(int id)
        {
            var typeEquip = await _context.TypeEquips.FirstOrDefaultAsync(te => te.idtypequip == id);
            return typeEquip is null ? null : new TypeEquipDTO
            {
                idtypequip = typeEquip.idtypequip,
                codetype = typeEquip.codetype ?? "",
                designation = typeEquip.designation
            };
        }

        public async Task<TypeEquipDTO> CreateTypeEquipAsync(CreateTypeEquipDTO typeEquipDto)
        {
            var typeEquip = new TypeEquip
            {
                designation = typeEquipDto.designation
            };

            _context.TypeEquips.Add(typeEquip);
            await _context.SaveChangesAsync();

            return new TypeEquipDTO
            {
                idtypequip = typeEquip.idtypequip,
                codetype = typeEquip.codetype ?? "",
                designation = typeEquip.designation
            };
        }

        public async Task<TypeEquipDTO?> UpdateTypeEquipAsync(int id, UpdateTypeEquipDTO typeEquipDto)
        {
            var typeEquip = await _context.TypeEquips.FirstOrDefaultAsync(te => te.idtypequip == id);
            if (typeEquip is null) return null;

            typeEquip.designation = typeEquipDto.designation;

            await _context.SaveChangesAsync();
            await _context.Entry(typeEquip).ReloadAsync();

            return new TypeEquipDTO
            {
                idtypequip = typeEquip.idtypequip,
                codetype = typeEquip.codetype ?? "",
                designation = typeEquip.designation
            };
        }

        public async Task<bool> CanDeleteTypeEquipAsync(int id)
        {
            return !await _context.Equipements.AnyAsync(e => e.idType == id);
        }

        public async Task<bool> DeleteTypeEquipAsync(int id)
        {
            if (!await CanDeleteTypeEquipAsync(id))
                return false;

            var typeEquip = await _context.TypeEquips.FirstOrDefaultAsync(te => te.idtypequip == id);
            if (typeEquip is null)
                return false;

            _context.TypeEquips.Remove(typeEquip);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
