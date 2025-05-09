using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public class OrganeEquipementService : IOrganeEquipementService
    {
        private readonly ApplicationDbContext _context;

        public OrganeEquipementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrganeEquipementDTO>> GetByEquipementIdAsync(int ideqpt)
        {
            return await _context.OrganeEquipements
                .Where(oe => oe.ideqpt == ideqpt)
                .Select(oe => new OrganeEquipementDTO
                {
                    idorg = oe.idorg,
                    ideqpt = oe.ideqpt,
                    numsérie = oe.numsérie,
                    nomOrgane = oe.Organe.libelle_organe
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<OrganeEquipementDTO>> GetByTypeAndMarqueAsync(int typeId, int marqueId)
        {
            return await _context.OrganeEquipements
                .Where(oe => oe.Equipement!.idType == typeId && oe.Equipement.idMarq == marqueId)
                .Select(oe => new OrganeEquipementDTO
                {
                    idorg = oe.idorg,
                    ideqpt = oe.ideqpt,
                    numsérie = oe.numsérie,
                    nomOrgane = oe.Organe.libelle_organe
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<OrganeEquipementDTO>> CreateAsync(CreateOrganeEquipementDTO dto)
        {
            // Get the equipment to find its groupe ID
            var equipement = await _context.Equipements
                .Include(e => e.GroupeIdentique)
                .FirstOrDefaultAsync(e => e.idEqpt == dto.ideqpt);

            if (equipement == null)
                throw new ArgumentException("Equipment not found");

            if (!equipement.idGrpIdq.HasValue)
                throw new ArgumentException("Equipment must be associated with a group");

            // Create OrganeEquipement entries
            var organeEquipements = dto.Organes.Select(o => new OrganeEquipement
            {
                ideqpt = dto.ideqpt,
                idorg = o.idorg,
                numsérie = o.numsérie
            }).ToList();

            _context.OrganeEquipements.AddRange(organeEquipements);
            await _context.SaveChangesAsync();

            // Create GroupeOrgane entries
            var existingOrgans = await _context.GroupeOrganes
                .Where(go => go.idgrpidq == equipement.idGrpIdq.Value && 
                            dto.Organes.Select(o => o.idorg).Contains(go.idorg))
                .Select(go => go.idorg)
                .ToListAsync();

            var newGroupeOrganes = dto.Organes
                .Where(o => !existingOrgans.Contains(o.idorg))
                .Select(o => new GroupeOrgane
                {
                    idgrpidq = equipement.idGrpIdq.Value,
                    idorg = o.idorg
                }).ToList();

            if (newGroupeOrganes.Any())
            {
                _context.GroupeOrganes.AddRange(newGroupeOrganes);
                await _context.SaveChangesAsync();
            }

            return await GetByEquipementIdAsync(dto.ideqpt);
        }
    }
} 