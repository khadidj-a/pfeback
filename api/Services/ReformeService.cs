using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public class ReformeService : IReformeService
    {
        private readonly ApplicationDbContext _context;

        public ReformeService(ApplicationDbContext context)
        {
            _context = context;
        }

      
       public async Task<IEnumerable<ReformeDTO>> GetAllAsync(string? search = null, string? sortBy = null, string? order = "asc")
{
    var query = _context.Reformes
        .Include(r => r.Equipement)
        .AsQueryable();

    // Recherche
    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(r =>
            r.numdes.ToString().Contains(search)||
            r.motifref.Contains(search) ||
            r.Equipement.codeEqp.Contains(search) ||
            r.Equipement.design.Contains(search)
        );
    }

    // Tri
    if (!string.IsNullOrEmpty(sortBy))
    {
        switch (sortBy.ToLower())
        {
            case "date":
                query = order == "desc" ? query.OrderByDescending(r => r.dateref) : query.OrderBy(r => r.dateref);
                break;
            case "numero":
    query = order == "desc"
        ? query.OrderByDescending(r => Convert.ToDecimal(r.numdes))
        : query.OrderBy(r => Convert.ToDecimal(r.numdes));
    break;

            case "motif":
                query = order == "desc" ? query.OrderByDescending(r => r.motifref) : query.OrderBy(r => r.motifref);
                break;
            case "codeequipement":
                query = order == "desc" ? query.OrderByDescending(r => r.Equipement.codeEqp) : query.OrderBy(r => r.Equipement.codeEqp);
                break;
            case "designation":
                query = order == "desc" ? query.OrderByDescending(r => r.Equipement.design) : query.OrderBy(r => r.Equipement.design);
                break;
        }
    }

    // Mapping vers DTO sans exposer les données de l’équipement
    return await query.Select(r => new ReformeDTO
    {
        idref = r.idref,
        ideqpt = r.ideqpt,
        motifref = r.motifref,
        dateref = r.dateref,
        numdes = r.numdes
    }).ToListAsync();
}
 public async Task<IEnumerable<ReformeDTO>> GetByUniteAsync(int idUnite, string? search = null, string? sortBy = null, string? order = "asc")
{
    var query = _context.Reformes
        .Include(r => r.Equipement)
        .Where(r => r.Equipement.idunite == idUnite)
        .AsQueryable();

    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(r =>
          r.numdes.ToString().Contains(search) ||
            r.motifref.Contains(search) ||
            r.Equipement.codeEqp.Contains(search) ||
            r.Equipement.design.Contains(search));
    }

    if (!string.IsNullOrEmpty(sortBy))
    {
        switch (sortBy.ToLower())
        {
            case "date":
                query = order == "desc" ? query.OrderByDescending(r => r.dateref) : query.OrderBy(r => r.dateref);
                break;
            case "numero":
    query = order == "desc"
        ? query.OrderByDescending(r => Convert.ToDecimal(r.numdes))
        : query.OrderBy(r => Convert.ToDecimal(r.numdes));
    break;

            case "motif":
                query = order == "desc" ? query.OrderByDescending(r => r.motifref) : query.OrderBy(r => r.motifref);
                break;
            case "equipement":
            case "ideqpt":
                query = order == "desc" ? query.OrderByDescending(r => r.ideqpt) : query.OrderBy(r => r.ideqpt);
                break;
            case "codeeqp":
                query = order == "desc" ? query.OrderByDescending(r => r.Equipement.codeEqp) : query.OrderBy(r => r.Equipement.codeEqp);
                break;
            case "design":
                query = order == "desc" ? query.OrderByDescending(r => r.Equipement.design) : query.OrderBy(r => r.Equipement.design);
                break;
        }
    }

    return await query.Select(r => new ReformeDTO
    {
        idref = r.idref,
        ideqpt = r.ideqpt,
        motifref = r.motifref,
        dateref = r.dateref,
        numdes = r.numdes
    }).ToListAsync();
}

        public async Task<ReformeDTO?> GetByIdAsync(int id)
        {
            var r = await _context.Reformes.FirstOrDefaultAsync(r => r.idref == id);

            return r == null ? null : new ReformeDTO
            {
                idref = r.idref,
                ideqpt = r.ideqpt,
                motifref = r.motifref,
                dateref = r.dateref,
                numdes = r.numdes
            };
        }

public async Task<ReformeDTO> CreateAsync(CreateReformeDTO dto)
{
    var equipement = await _context.Equipements.FindAsync(dto.ideqpt);
    if (equipement == null)
        throw new Exception("Équipement introuvable");

    if (equipement.etat == "reforme") // vérifie en minuscules
        throw new Exception($"L’équipement est déjà en état '{equipement.etat}' et ne peut pas être réformé à nouveau.");

    // ✅ Mise à jour avec la bonne valeur attendue par la contrainte CHECK
    equipement.etat = "reforme";
    _context.Equipements.Update(equipement);

    var r = new Reforme
    {
        ideqpt = dto.ideqpt,
        motifref = dto.motifref,
        dateref = dto.dateref,
        numdes = dto.numdes
    };

    _context.Reformes.Add(r);
    await _context.SaveChangesAsync();

    return await GetByIdAsync(r.idref) ?? throw new Exception("Création échouée");
}

        public async Task<int> GetReformeCountAsync()
        {
            return await _context.Reformes.CountAsync();
        }
        public async Task<string?> GetEtatEquipementAsync(int ideqpt)
{
    var equipement = await _context.Equipements
                                   .Where(e => e.idEqpt == ideqpt)
                                   .Select(e => e.etat)
                                   .FirstOrDefaultAsync();

    return equipement; // Retourne null si non trouvé
}
public async Task<bool> NumeroDecisionExistsAsync(string numeroDecision)
{
    if (!int.TryParse(numeroDecision, out int numero))
        return false; // ou throw une exception selon ton besoin

    return await _context.Reformes.AnyAsync(r => r.numdes == numero);
}


    }
}
