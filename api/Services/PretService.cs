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
        .Include(p => p.UniteEmettrice)
        .Include(p => p.UniteDestination)
        .AsQueryable();

    // 🔎 Filtrage par unité émettrice
    if (idUnite.HasValue)
    {
        query = query.Where(p => p.iduniteemt == idUnite.Value);
    }

    // 🔍 Recherche sur tous les champs demandés
    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(p =>
            (p.Equipement != null && (
                p.Equipement.codeEqp.Contains(search) ||       // Code équipement
                p.Equipement.design.Contains(search) ||        // Désignation équipement
                p.Equipement.position_physique.Contains(search) // Position physique
            )) ||
            (p.UniteEmettrice != null && p.UniteEmettrice.designation.Contains(search)) || // Unité actuelle
            (p.UniteDestination != null && p.UniteDestination.designation.Contains(search)) || // Destination
            (p.motif != null && p.motif.Contains(search)) ||
            p.datepret.ToString().Contains(search) ||
            p.duree.ToString().Contains(search)
        );
    }

    // 📊 Tri
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
                query = order == "desc"
                    ? query.OrderByDescending(p => p.Equipement!.design)
                    : query.OrderBy(p => p.Equipement!.design);
                break;

            case "codeequipement":
                query = order == "desc"
                    ? query.OrderByDescending(p => p.Equipement!.codeEqp)
                    : query.OrderBy(p => p.Equipement!.codeEqp);
                break;

            case "positionphysique":
                query = order == "desc"
                    ? query.OrderByDescending(p => p.Equipement!.position_physique)
                    : query.OrderBy(p => p.Equipement!.position_physique);
                break;

            case "unite":
                query = order == "desc"
                    ? query.OrderByDescending(p => p.UniteEmettrice!.designation)
                    : query.OrderBy(p => p.UniteEmettrice!.designation);
                break;

            case "unitedestination":
                query = order == "desc"
                    ? query.OrderByDescending(p => p.UniteDestination!.designation)
                    : query.OrderBy(p => p.UniteDestination!.designation);
                break;

            case "motif":
                query = order == "desc"
                    ? query.OrderByDescending(p => p.motif)
                    : query.OrderBy(p => p.motif);
                break;
        }
    }

    return await query.ToListAsync();
}
public async Task<IEnumerable<PretDTO>> GetByUniteAsync(int idUnite, string? search = null, string? sortBy = null, string? order = "asc")
{
    var query = _context.Prets
        .Include(p => p.Equipement)
        .Include(p => p.UniteEmettrice)
        .Include(p => p.UniteDestination)
        .Where(p => p.iduniteemt == idUnite || p.idunite == idUnite) // l'unité du responsable
        .AsQueryable();

    // 🔍 Recherche enrichie
    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(p =>
            (p.Equipement != null && (
                p.Equipement.codeEqp.Contains(search) ||
                p.Equipement.design.Contains(search) ||
                p.Equipement.position_physique.Contains(search)
            )) ||
            (p.UniteEmettrice != null && p.UniteEmettrice.designation.Contains(search)) ||
            (p.UniteDestination != null && p.UniteDestination.designation.Contains(search)) ||
            (p.motif != null && p.motif.Contains(search)) ||
            p.datepret.ToString().Contains(search) ||
            p.duree.ToString().Contains(search)
        );
    }

    // 🔃 Tri enrichi
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
                query = order == "desc" ? query.OrderByDescending(p => p.Equipement.design) : query.OrderBy(p => p.Equipement.design);
                break;
            case "codeequipement":
                query = order == "desc" ? query.OrderByDescending(p => p.Equipement.codeEqp) : query.OrderBy(p => p.Equipement.codeEqp);
                break;
            case "positionphysique":
                query = order == "desc" ? query.OrderByDescending(p => p.Equipement.position_physique) : query.OrderBy(p => p.Equipement.position_physique);
                break;
            case "unite":
                query = order == "desc" ? query.OrderByDescending(p => p.UniteEmettrice.designation) : query.OrderBy(p => p.UniteEmettrice.designation);
                break;
            case "unitedestination":
                query = order == "desc" ? query.OrderByDescending(p => p.UniteDestination.designation) : query.OrderBy(p => p.UniteDestination.designation);
                break;
            case "motif":
                query = order == "desc" ? query.OrderByDescending(p => p.motif) : query.OrderBy(p => p.motif);
                break;
        }
    }

    return await query.Select(p => new PretDTO
    {
        idpret = p.idpret,
        ideqpt = p.ideqpt,
        iduniteemt = p.iduniteemt,
        idunite = p.idunite,
        datepret = p.datepret,
        duree = p.duree,
        motif = p.motif
    }).ToListAsync();
}

public async Task<Pret> CreateAsync(CreatePretDTO dto, string role)
{
    var equipement = await _context.Equipements.FindAsync(dto.ideqpt);
    if (equipement == null)
        throw new Exception("Équipement introuvable");

    if (equipement.etat == "reforme")
        throw new Exception("Équipement réformé, prêt non autorisé");


    if (role == "Responsable Unité" && equipement.idunite != dto.idunite)
        throw new Exception("Impossible d’effectuer un prêt d’un équipement appartenant à une autre unité.");

    var uniteDest = await _context.Unites.FindAsync(dto.idunite);
    if (uniteDest == null)
        throw new Exception("Unité de destination introuvable");

    var pret = new Pret
    {
        ideqpt = dto.ideqpt,
        idunite = dto.idunite,
        iduniteemt = equipement.idunite,
        duree = dto.duree,
        datepret = dto.datepret,
        motif = dto.motif
    };

    _context.Prets.Add(pret);

    //  Mise à jour de la position physique
    equipement.position_physique = uniteDest.designation;
    _context.Equipements.Update(equipement);

    await _context.SaveChangesAsync();

    return pret;
}


        public async Task<Pret?> GetByIdAsync(int id)
        {
            return await _context.Prets.Include(p => p.Equipement).FirstOrDefaultAsync(p => p.idpret == id);
        }
          public async Task<int> GetPretCountAsync()
        {
            return await _context.Prets.CountAsync();
        }

        
            public async Task<string?> GetEtatEquipementAsync(int idEquipement)
{
    var equipement = await _context.Equipements
                                   .Where(e => e.idEqpt == idEquipement)
                                   .Select(e => e.etat)
                                   .FirstOrDefaultAsync();

    return equipement; // Retourne null si non trouvé
}public async Task MettreAJourPositionPhysiqueAsync()
{
    var aujourdHui = DateTime.Now.Date;

    // Récupérer tous les prêts avec les équipements liés
    var prets = await _context.Prets
        .Include(p => p.Equipement)
        .ToListAsync();

    foreach (var pret in prets)
    {
        // Date de fin du prêt (datepret + durée en jours)
        var dateFinPret = pret.datepret.AddDays(pret.duree);

        if (pret.Equipement != null)
        {
            if (aujourdHui >= dateFinPret)
            {
                // Le prêt est terminé : remettre la position physique à l'unité émettrice
                var uniteEmettrice = await _context.Unites.FindAsync(pret.iduniteemt);
                if (uniteEmettrice != null)
                {
                    pret.Equipement.position_physique = uniteEmettrice.designation;
                }
            }
            else
            {
                // Prêt encore actif : position physique = unité destination
                var uniteDestination = await _context.Unites.FindAsync(pret.idunite);
                if (uniteDestination != null)
                {
                    pret.Equipement.position_physique = uniteDestination.designation;
                }
            }

            _context.Equipements.Update(pret.Equipement);
        }
    }

    await _context.SaveChangesAsync();
}


    }
}

