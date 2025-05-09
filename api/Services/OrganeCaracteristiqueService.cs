using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public class OrganeCaracteristiqueService : IOrganeCaracteristiqueService
    {
        private readonly ApplicationDbContext _context;

        public OrganeCaracteristiqueService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrganeCaracteristiqueDTO>> GetByOrganeIdAsync(int id_organe)
        {
            return await _context.OrganeCaracteristiques
                .Where(oc => oc.id_organe == id_organe)
                .Select(oc => new OrganeCaracteristiqueDTO
                {
                    id_organe = oc.id_organe,
                    id_caracteristique = oc.id_caracteristique,
                    valeur = oc.valeur,
                    nomCaracteristique = oc.Caracteristique.libelle
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<OrganeCaracteristiqueDTO>> CreateAsync(CreateOrganeCaracteristiqueDTO dto)
        {
            var organeCaracteristiques = dto.Caracteristiques.Select(c => new OrganeCaracteristique
            {
                id_organe = dto.id_organe,
                id_caracteristique = c.idcarac,
                valeur = c.valeur.ToString()
            }).ToList();

            _context.OrganeCaracteristiques.AddRange(organeCaracteristiques);
            await _context.SaveChangesAsync();

            return organeCaracteristiques.Select(oc => new OrganeCaracteristiqueDTO
            {
                id_organe = oc.id_organe,
                id_caracteristique = oc.id_caracteristique,
                valeur = oc.valeur,
                nomCaracteristique = oc.Caracteristique.libelle
            });
        }

        public async Task<OrganeCaracteristiqueDTO?> UpdateAsync(int id_organe, int id_caracteristique, UpdateOrganeCaracteristiqueDTO dto)
        {
            var organeCaracteristique = await _context.OrganeCaracteristiques
                .Include(oc => oc.Caracteristique)
                .FirstOrDefaultAsync(oc => oc.id_organe == id_organe && oc.id_caracteristique == id_caracteristique);

            if (organeCaracteristique == null)
                return null;

            organeCaracteristique.valeur = dto.valeur;
            await _context.SaveChangesAsync();

            return new OrganeCaracteristiqueDTO
            {
                id_organe = organeCaracteristique.id_organe,
                id_caracteristique = organeCaracteristique.id_caracteristique,
                valeur = organeCaracteristique.valeur,
                nomCaracteristique = organeCaracteristique.Caracteristique.libelle
            };
        }
    }
} 