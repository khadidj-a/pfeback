using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFE_PROJECT.Services
{
    public class UniteService : IUniteService
    {
        private readonly ApplicationDbContext _context;

        public UniteService(ApplicationDbContext context)
        {
            _context = context;
        }

       public async Task<IEnumerable<UniteDTO>> GetAllUnitesAsync(string? searchTerm = null, string? sortBy = null, bool ascending = true)
{
    var query = _context.Unites
        .Include(u => u.Wilaya)
        .Include(u => u.Region)
        .AsQueryable();

    // 🔍 Filtrage insensible à la casse
    if (!string.IsNullOrEmpty(searchTerm))
    {
        string lowerSearch = searchTerm.ToLower();
        query = query.Where(u =>
            u.designation.ToLower().Contains(lowerSearch) ||
            (u.codeunite != null && u.codeunite.ToLower().Contains(lowerSearch)) ||
            u.Wilaya.nomwilaya.ToLower().Contains(lowerSearch) ||
            u.Region.nomregion.ToLower().Contains(lowerSearch));
    }

    // 🧭 Tri
    sortBy ??= "idunite";
    query = sortBy.ToLower() switch
    {
        "designation" => ascending ? query.OrderBy(u => u.designation) : query.OrderByDescending(u => u.designation),
        "codeunite" => ascending ? query.OrderBy(u => u.codeunite) : query.OrderByDescending(u => u.codeunite),
        "wilaya" => ascending ? query.OrderBy(u => u.Wilaya.nomwilaya) : query.OrderByDescending(u => u.Wilaya.nomwilaya),
        "region" => ascending ? query.OrderBy(u => u.Region.nomregion) : query.OrderByDescending(u => u.Region.nomregion),
        _ => ascending ? query.OrderBy(u => u.idunite) : query.OrderByDescending(u => u.idunite)
    };

    // 🟢 Projection vers DTO
    var result = await query.Select(u => new UniteDTO
    {
        idunite = u.idunite,
        codeunite = u.codeunite ?? "",
        designation = u.designation,
        idwilaya = u.idwilaya,
        nomwilaya = u.Wilaya.nomwilaya,
        idregion = u.idregion,
        nomregion = u.Region.nomregion
    }).ToListAsync();

    return result;
}


        public async Task<UniteDTO?> GetUniteByIdAsync(int id)
        {
            var unite = await _context.Unites
                .Include(u => u.Wilaya)
                .Include(u => u.Region)
                .FirstOrDefaultAsync(u => u.idunite == id);

            return unite == null ? null : new UniteDTO
            {
                idunite = unite.idunite,
                codeunite = unite.codeunite ?? "",
                designation = unite.designation,
                idwilaya = unite.idwilaya,
                nomwilaya = unite.Wilaya.nomwilaya,
                idregion = unite.idregion,
                nomregion = unite.Region.nomregion
            };
        }

        public async Task<UniteDTO> CreateUniteAsync(CreateUniteDTO dto)
        {
            var unite = new Unite
            {
                designation = dto.designation,
                idwilaya = dto.idwilaya,
                idregion = dto.idregion
            };

            _context.Unites.Add(unite);
            await _context.SaveChangesAsync();
            await _context.Entry(unite).ReloadAsync(); // pour récupérer le code généré

            return await GetUniteByIdAsync(unite.idunite) ?? throw new Exception("Unité non trouvée après création.");
        }

        public async Task<UniteDTO?> UpdateUniteAsync(int id, UpdateUniteDTO dto)
        {
            var unite = await _context.Unites.FindAsync(id);
            if (unite == null) return null;

            unite.designation = dto.designation;
            unite.idwilaya = dto.idwilaya;
            unite.idregion = dto.idregion;

            await _context.SaveChangesAsync();
            await _context.Entry(unite).ReloadAsync();

            return await GetUniteByIdAsync(id);
        }

        public async Task<bool> CanDeleteUniteAsync(int id)
{
    // On vérifie dans la table des affectations s'il y a un équipement affecté à cette unité
    return !await _context.Affectations.AnyAsync(a => a.idunite == id);
}


        public async Task<bool> DeleteUniteAsync(int id)
        {
            if (!await CanDeleteUniteAsync(id)) return false;

            var unite = await _context.Unites.FindAsync(id);
            if (unite == null) return false;

            _context.Unites.Remove(unite);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Wilaya>> GetAllWilayasAsync() => await _context.Wilayas.ToListAsync();
        public async Task<IEnumerable<Region>> GetAllRegionsAsync() => await _context.Regions.ToListAsync();
    }
}
