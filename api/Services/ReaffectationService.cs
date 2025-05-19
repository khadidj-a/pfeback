using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public class ReaffectationService : IReaffectationService
    {
        private readonly ApplicationDbContext _context;

        public ReaffectationService(ApplicationDbContext context)
        {
            _context = context;
        }
public async Task<IEnumerable<ReaffectationDTO>> GetAllAsync(string? search = null, string? sortBy = null, string? order = "asc")
{
    var query = _context.Reaffectations
        .Include(r => r.Equipement)
        .Include(r => r.UniteEmission)
        .Include(r => r.UniteDestination)
        .AsQueryable();

    // 🔍 Recherche enrichie
    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(r =>
            r.motifreaf.Contains(search) ||
            r.Equipement.design.Contains(search) ||
            r.Equipement.codeEqp.Contains(search) ||
            r.UniteEmission.designation.Contains(search) ||
            r.UniteDestination.designation.Contains(search)
        );
    }

    // 🔃 Tri enrichi
    if (!string.IsNullOrEmpty(sortBy))
    {
        switch (sortBy.ToLower())
        {
            case "date":
                query = order == "desc" ? query.OrderByDescending(r => r.datereaf) : query.OrderBy(r => r.datereaf);
                break;
            case "motif":
                query = order == "desc" ? query.OrderByDescending(r => r.motifreaf) : query.OrderBy(r => r.motifreaf);
                break;
            case "equipement":
            case "designation":
                query = order == "desc" ? query.OrderByDescending(r => r.Equipement.design) : query.OrderBy(r => r.Equipement.design);
                break;
            case "code":
                query = order == "desc" ? query.OrderByDescending(r => r.Equipement.codeEqp) : query.OrderBy(r => r.Equipement.codeEqp);
                break;
            case "ancienneunite":
                query = order == "desc" ? query.OrderByDescending(r => r.UniteEmission.designation) : query.OrderBy(r => r.UniteEmission.designation);
                break;
            case "nouvelleunite":
                query = order == "desc" ? query.OrderByDescending(r => r.UniteDestination.designation) : query.OrderBy(r => r.UniteDestination.designation);
                break;
        }
    }

    return await query.Select(r => new ReaffectationDTO
    {
        idreaf = r.idreaf,
        ideqpt = r.ideqpt,
        iduniteemt = r.iduniteemt,
        idunitedest = r.idunitedest,
        datereaf = r.datereaf,
        motifreaf = r.motifreaf
    }).ToListAsync();
}


public async Task<ReaffectationDTO> CreateAsync(CreateReaffectationDTO dto)
{
    var equipement = await _context.Equipements.FindAsync(dto.idEquipement);
    if (equipement == null)
        throw new Exception("Équipement introuvable");

    if (equipement.etat == "reforme" )
        throw new Exception($"L’équipement est en état '{equipement.etat}' et ne peut pas être réaffecté.");

    // 🔸 On garde en mémoire l’unité actuelle AVANT la modification
   int idUniteEmettrice = equipement.idunite.GetValueOrDefault();


    // 🔸 Création de la réaffectation AVANT de modifier l’équipement
    var reaffectation = new Reaffectation
    {
        ideqpt = dto.idEquipement,
        iduniteemt = idUniteEmettrice,
        idunitedest = dto.idUniteDestination,
        datereaf = dto.date,
        motifreaf = dto.motif
    };

    _context.Reaffectations.Add(reaffectation);

    // 🔸 Mise à jour de l’équipement (après avoir récupéré l’unité précédente)
    equipement.idunite = dto.idUniteDestination;
    _context.Equipements.Update(equipement);

    await _context.SaveChangesAsync();

    return new ReaffectationDTO
    {
        idreaf = reaffectation.idreaf,
        ideqpt = reaffectation.ideqpt,
        iduniteemt = reaffectation.iduniteemt,
        idunitedest = reaffectation.idunitedest,
        datereaf = reaffectation.datereaf,
        motifreaf = reaffectation.motifreaf
    };
}

public async Task<ReaffectationDTO?> GetByIdAsync(int id)
{
    var r = await _context.Reaffectations.FirstOrDefaultAsync(r => r.idreaf == id);

    return r == null ? null : new ReaffectationDTO
    {
        idreaf = r.idreaf,
        ideqpt = r.ideqpt,
        iduniteemt = r.iduniteemt,
        idunitedest = r.idunitedest,
        datereaf = r.datereaf,
        motifreaf = r.motifreaf
    };
}public async Task<IEnumerable<ReaffectationDTO>> GetByUniteAsync(int idUnite, string? search = null, string? sortBy = null, string? order = "asc")
{
    var query = _context.Reaffectations
        .Include(r => r.Equipement)
        .Include(r => r.UniteEmission)
        .Include(r => r.UniteDestination)
        .Where(r => r.iduniteemt == idUnite )
        .AsQueryable();

    // 🔍 Recherche enrichie
    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(r =>
            r.motifreaf.Contains(search) ||
            r.Equipement.design.Contains(search) ||
            r.Equipement.codeEqp.Contains(search) ||
            r.UniteEmission.designation.Contains(search) ||
            r.UniteDestination.designation.Contains(search)
        );
    }

    // 🔃 Tri enrichi
    if (!string.IsNullOrEmpty(sortBy))
    {
        switch (sortBy.ToLower())
        {
            case "date":
                query = order == "desc" ? query.OrderByDescending(r => r.datereaf) : query.OrderBy(r => r.datereaf);
                break;
            case "motif":
                query = order == "desc" ? query.OrderByDescending(r => r.motifreaf) : query.OrderBy(r => r.motifreaf);
                break;
            case "equipement":
            case "designation":
                query = order == "desc" ? query.OrderByDescending(r => r.Equipement.design) : query.OrderBy(r => r.Equipement.design);
                break;
            case "code":
                query = order == "desc" ? query.OrderByDescending(r => r.Equipement.codeEqp) : query.OrderBy(r => r.Equipement.codeEqp);
                break;
            case "ancienneunite":
                query = order == "desc" ? query.OrderByDescending(r => r.UniteEmission.designation) : query.OrderBy(r => r.UniteEmission.designation);
                break;
            case "nouvelleunite":
                query = order == "desc" ? query.OrderByDescending(r => r.UniteDestination.designation) : query.OrderBy(r => r.UniteDestination.designation);
                break;
        }
    }

    return await query.Select(r => new ReaffectationDTO
    {
        idreaf = r.idreaf,
        ideqpt = r.ideqpt,
        iduniteemt = r.iduniteemt,
        idunitedest = r.idunitedest,
        datereaf = r.datereaf,
        motifreaf = r.motifreaf
    }).ToListAsync();
}

   public async Task<int> GetReaffectationCountAsync()
    {
      return await _context.Reaffectations.CountAsync();
    }
            public async Task<string?> GetEtatEquipementAsync(int idEquipement)
{
    var equipement = await _context.Equipements
                                   .Where(e => e.idEqpt == idEquipement)
                                   .Select(e => e.etat)
                                   .FirstOrDefaultAsync();

    return equipement; // Retourne null si non trouvé
}

    }
}
