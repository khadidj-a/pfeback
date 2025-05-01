using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public class GroupeCaracteristiqueService : IGroupeCaracteristiqueService
    {
        private readonly ApplicationDbContext _context;

        public GroupeCaracteristiqueService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GroupeCaracteristiqueDTO>> GetAllAsync()
        {
            return await _context.GroupeCaracteristiques
                .Include(gc => gc.Caracteristique)
                .Select(gc => new GroupeCaracteristiqueDTO
                {
                    idgrpidq = gc.idgrpidq,
                    idcarac = gc.idcarac,
                    libelleCaracteristique = gc.Caracteristique.libelle
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<GroupeCaracteristiqueDTO>> GetByGroupeIdAsync(int idgrpidq)
        {
            return await _context.GroupeCaracteristiques
                .Where(gc => gc.idgrpidq == idgrpidq)
                .Include(gc => gc.Caracteristique)
                .Select(gc => new GroupeCaracteristiqueDTO
                {
                    idgrpidq = gc.idgrpidq,
                    idcarac = gc.idcarac,
                    libelleCaracteristique = gc.Caracteristique.libelle
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<GroupeCaracteristiqueDTO>> CreateAsync(int idgrpidq, List<int> idcaracs)
        {
            var groupeCaracteristiques = idcaracs.Select(idcarac => new GroupeCaracteristique
            {
                idgrpidq = idgrpidq,
                idcarac = idcarac
            }).ToList();

            _context.GroupeCaracteristiques.AddRange(groupeCaracteristiques);
            await _context.SaveChangesAsync();

            return await GetByGroupeIdAsync(idgrpidq);
        }

        public async Task<bool> DeleteByGroupeIdAsync(int idgrpidq)
        {
            var groupeCaracteristiques = await _context.GroupeCaracteristiques
                .Where(gc => gc.idgrpidq == idgrpidq)
                .ToListAsync();

            if (!groupeCaracteristiques.Any()) return false;

            _context.GroupeCaracteristiques.RemoveRange(groupeCaracteristiques);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 