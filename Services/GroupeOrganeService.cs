using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public class GroupeOrganeService : IGroupeOrganeService
    {
        private readonly ApplicationDbContext _context;

        public GroupeOrganeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GroupeOrganeDTO>> GetAllAsync()
        {
            return await _context.GroupeOrganes
                .Include(go => go.Organe)
                .Select(go => new GroupeOrganeDTO
                {
                    idgrpidq = go.idgrpidq,
                    idorg = go.idorg,
                    libelleOrgane = go.Organe.libelle_organe
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<GroupeOrganeDTO>> GetByGroupeIdAsync(int idgrpidq)
        {
            return await _context.GroupeOrganes
                .Where(go => go.idgrpidq == idgrpidq)
                .Include(go => go.Organe)
                .Include(go => go.GroupeIdentique)
                .Select(go => new GroupeOrganeDTO
                {
                    idgrpidq = go.idgrpidq,
                    idorg = go.idorg,
                    libelleOrgane = go.Organe.libelle_organe
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<GroupeOrganeDTO>> CreateAsync(int idgrpidq, List<int> idorgs)
        {
            var groupeOrganes = idorgs.Select(idorg => new GroupeOrgane
            {
                idgrpidq = idgrpidq,
                idorg = idorg
            }).ToList();

            _context.GroupeOrganes.AddRange(groupeOrganes);
            await _context.SaveChangesAsync();

            return await GetByGroupeIdAsync(idgrpidq);
        }

        public async Task<bool> DeleteByGroupeIdAsync(int idgrpidq)
        {
            var groupeOrganes = await _context.GroupeOrganes
                .Where(go => go.idgrpidq == idgrpidq)
                .ToListAsync();

            if (!groupeOrganes.Any()) return false;

            _context.GroupeOrganes.RemoveRange(groupeOrganes);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 