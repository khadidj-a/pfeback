using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFE_PROJECT.Services
{
    public class CaracteristiqueService : ICaracteristiqueService
    {
        private readonly ApplicationDbContext _context;

        public CaracteristiqueService(ApplicationDbContext context)
        {
            _context = context;
        }

       public async Task<IEnumerable<CaracteristiqueDTO>> GetAllCaracteristiquesAsync(string? searchTerm, string? sortBy, bool ascending)
{
    var query = _context.Caracteristiques.AsQueryable();

    if (!string.IsNullOrWhiteSpace(searchTerm))
    {
        query = query.Where(c => c.libelle.ToLower().Contains(searchTerm.ToLower()));
    }

    // Tri dynamique
    query = sortBy switch
    {
        "libelle" => ascending ? query.OrderBy(c => c.libelle) : query.OrderByDescending(c => c.libelle),
        _ => ascending ? query.OrderBy(c => c.id_caracteristique) : query.OrderByDescending(c => c.id_caracteristique)
    };

    var list = await query.ToListAsync();
    return list.Select(c => new CaracteristiqueDTO
    {
        id_caracteristique= c.id_caracteristique,
        libelle = c.libelle
    });
}

        public async Task<CaracteristiqueDTO?> GetCaracteristiqueByIdAsync(int id)
        {
            var carac = await _context.Caracteristiques.FindAsync(id);
            return carac == null ? null : new CaracteristiqueDTO
            {
                id_caracteristique = carac.id_caracteristique,
                libelle = carac.libelle
            };
        }

        public async Task<CaracteristiqueDTO> CreateCaracteristiqueAsync(CreateCaracteristiqueDTO dto)
        {
            var carac = new Caracteristique { libelle = dto.libelle };
            _context.Caracteristiques.Add(carac);
            await _context.SaveChangesAsync();
            return new CaracteristiqueDTO { id_caracteristique = carac.id_caracteristique, libelle = carac.libelle };
        }

        public async Task<CaracteristiqueDTO?> UpdateCaracteristiqueAsync(int id, UpdateCaracteristiqueDTO dto)
        {
            var carac = await _context.Caracteristiques.FindAsync(id);
            if (carac == null) return null;
            carac.libelle = dto.libelle;
            await _context.SaveChangesAsync();
            return new CaracteristiqueDTO { id_caracteristique = carac.id_caracteristique, libelle = carac.libelle };
        }

        public async Task<bool> CanDeleteCaracteristiqueAsync(int id)
        {
            return !await _context.CaracteristiqueEquipements.AnyAsync(ce => ce.idcarac == id);
        }

        public async Task<bool> DeleteCaracteristiqueAsync(int id)
        {
            if (!await CanDeleteCaracteristiqueAsync(id)) return false;
            var carac = await _context.Caracteristiques.FindAsync(id);
            if (carac == null) return false;
            _context.Caracteristiques.Remove(carac);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CaracteristiqueDTO>> GetByTypeAndMarqueAsync(int typeId, int marqueId)
        {
            var caracteristiques = await _context.CaracteristiqueEquipements
                .Where(ce => ce.Equipement!.idType == typeId && ce.Equipement.idMarq == marqueId)
                .Select(ce => new CaracteristiqueDTO
                {
                    id_caracteristique = ce.Caracteristique!.id_caracteristique,
                    libelle = ce.Caracteristique.libelle
                })
                .Distinct()
                .ToListAsync();

            return caracteristiques;
        }

           public async Task<int> GetCaracteristiqueCountAsync()
        {
            return await _context.Caracteristiques.CountAsync();
        }
    }
}

