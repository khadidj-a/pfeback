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
            var query = _context.Reformes.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(r =>
                    r.numdes.Contains(search) ||
                    r.motifref.Contains(search));
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "date":
                        query = order == "desc"
                            ? query.OrderByDescending(r => r.dateref)
                            : query.OrderBy(r => r.dateref);
                        break;

                    case "numero":
                        query = order == "desc"
                            ? query.OrderByDescending(r => r.numdes)
                            : query.OrderBy(r => r.numdes);
                        break;

                    case "motif":
                        query = order == "desc"
                            ? query.OrderByDescending(r => r.motifref)
                            : query.OrderBy(r => r.motifref);
                        break;

                    case "equipement":
                    case "ideqpt":
                        query = order == "desc"
                            ? query.OrderByDescending(r => r.ideqpt)
                            : query.OrderBy(r => r.ideqpt);
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

  public async Task<IEnumerable<Reforme>> GetByUniteAsync(int idUnite, string? search = null, string? sortBy = null, string? order = "asc")
{
    var query = from r in _context.Reformes
                join eq in _context.Equipements on r.ideqpt equals eq.idEqpt
                where eq.idunite == idUnite
                select r; // Récupère directement les entités Reforme

    // 🔍 Recherche
    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(r =>
            r.numdes.Contains(search) ||
            r.motifref.Contains(search));
    }

    // 🔃 Tri
    if (!string.IsNullOrEmpty(sortBy))
    {
        switch (sortBy.ToLower())
        {
            case "date":
                query = order == "desc"
                    ? query.OrderByDescending(r => r.dateref)
                    : query.OrderBy(r => r.dateref);
                break;

            case "numero":
                query = order == "desc"
                    ? query.OrderByDescending(r => r.numdes)
                    : query.OrderBy(r => r.numdes);
                break;

            case "motif":
                query = order == "desc"
                    ? query.OrderByDescending(r => r.motifref)
                    : query.OrderBy(r => r.motifref);
                break;

            case "equipement":
            case "ideqpt":
                query = order == "desc"
                    ? query.OrderByDescending(r => r.ideqpt)
                    : query.OrderBy(r => r.ideqpt);
                break;
        }
    }

    return await query.ToListAsync(); // Retourne des entités Reforme
}




        public async Task<ReformeDTO> CreateAsync(CreateReformeDTO dto)
        {
            var equipement = await _context.Equipements.FindAsync(dto.idEquipement);
            if (equipement == null)
                throw new Exception("Équipement introuvable");

            if (equipement.état == "Prêt")
                throw new Exception("L’équipement est en état 'Prêt' et ne peut pas être réformé.");

            // ✅ Mise à jour de l'état de l'équipement
            equipement.état = "Réformé";
            _context.Equipements.Update(equipement); // 💡 Important

            var r = new Reforme
            {
                ideqpt = dto.idEquipement,
                motifref = dto.motif,
                dateref = dto.dateReforme,
                numdes = dto.numeroDecision
            };

            _context.Reformes.Add(r);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(r.idref) ?? throw new Exception("Création échouée");
        }
    }
}
