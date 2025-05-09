using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public class PretService : IPretService
    {
        private readonly ApplicationDbContext _context;

        public PretService(ApplicationDbContext context)
        {
            _context = context;
        }
public async Task<IEnumerable<Pret>> GetAllAsync(string? search = null, string? sortBy = null, string? order = "asc", int? idUnite = null)
{
    var query = _context.Prets
        .Include(p => p.Equipement)
        .AsQueryable();

    // ✅ Filtrer par l’unité émettrice si c’est un responsable d’unité
    if (idUnite.HasValue)
    {
        query = query.Where(p => p.iduniteemt == idUnite.Value);
    }

    // Recherche
    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(p =>
            p.Equipement != null && p.Equipement.design.Contains(search));
    }

    // Tri
    if (!string.IsNullOrEmpty(sortBy))
    {
        switch (sortBy.ToLower())
        {
            case "date":
                query = order == "desc" ? query.OrderByDescending(p => p.datepret) : query.OrderBy(p => p.datepret);
                break;
            case "duree":
                query = order == "desc" ? query.OrderByDescending(p => p.duree) : query.OrderBy(p => p.duree);
                break;
            case "equipement":
            case "designation":
                query = order == "desc"
                    ? query.OrderByDescending(p => p.Equipement!.design)
                    : query.OrderBy(p => p.Equipement!.design);
                break;
            case "unite":
                query = order == "desc"
                    ? query.OrderByDescending(p => p.idunite)
                    : query.OrderBy(p => p.idunite);
                break;
        }
    }

    return await query.ToListAsync();
}

public async Task<Pret> CreateAsync(CreatePretDTO dto, string role)
{
    var equipement = await _context.Equipements.FindAsync(dto.ideqpt);
    if (equipement == null)
        throw new Exception("Équipement introuvable");

    if (equipement.état == "Réformé")
        throw new Exception("Équipement réformé, prêt non autorisé");

    if (equipement.état == "Prêt")
        throw new Exception("Équipement déjà en prêt");

    // 🔐 Vérification seulement pour Responsable Unité
    if (role == "Responsable Unité" && equipement.idunite != dto.idunite)
        throw new Exception("Impossible d’effectuer un prêt d’un équipement appartenant à une autre unité.");

    var pret = new Pret
    {
        ideqpt = dto.ideqpt,
        idunite = dto.idunite,
        iduniteemt = equipement.idunite,
        duree = dto.duree,
        datepret = dto.datepret
    };

    _context.Prets.Add(pret);

    equipement.état = "Prêt";
    _context.Equipements.Update(equipement);

    await _context.SaveChangesAsync();

    return pret;
}

        public async Task<Pret?> GetByIdAsync(int id)
        {
            return await _context.Prets.Include(p => p.Equipement).FirstOrDefaultAsync(p => p.idpret == id);
        }
    }
}

