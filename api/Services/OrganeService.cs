using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;
using PFE_PROJECT.DTOs;
namespace PFE_PROJECT.Services
{
    public class OrganeService : IOrganeService
    {
        private readonly ApplicationDbContext _context;

        public OrganeService(ApplicationDbContext context)
        {
            _context = context;
        }
public async Task<IEnumerable<OrganeDTO>> GetAllAsync(
    string? searchTerm = null,
    string? sortBy = null,
    bool ascending = true)
{
    var query = _context.Organes
        .Include(o => o.Marque)
        .Include(o => o.OrganeCaracteristiques)
            .ThenInclude(oc => oc.Caracteristique)
        .AsQueryable();

    // 🔍 Filtrage par mot-clé
    if (!string.IsNullOrEmpty(searchTerm))
    {
        var lowerSearch = searchTerm.ToLower();
        query = query.Where(o => 
            o.code_organe.ToLower().Contains(lowerSearch) ||
            o.libelle_organe.ToLower().Contains(lowerSearch) ||
            o.modele.ToLower().Contains(lowerSearch) ||
            o.Marque.nom_fabriquant.ToLower().Contains(lowerSearch));
    }

    // 🔍 Filtrage par marque

    // ⬆⬇ Tri
    sortBy ??= "id_organe";
    query = sortBy.ToLower() switch
    {
        "libelle_organe" => ascending ? query.OrderBy(o => o.libelle_organe) : query.OrderByDescending(o => o.libelle_organe),
        "code_organe" => ascending ? query.OrderBy(o => o.code_organe) : query.OrderByDescending(o => o.code_organe),
        "modele" => ascending ? query.OrderBy(o => o.modele) : query.OrderByDescending(o => o.modele),
        "nom_marque" => ascending ? query.OrderBy(o => o.Marque.nom_fabriquant) : query.OrderByDescending(o => o.Marque.nom_fabriquant),
        _ => ascending ? query.OrderBy(o => o.id_organe) : query.OrderByDescending(o => o.id_organe)
    };

    // 🟢 Projection vers DTO
    return await query.Select(o => new OrganeDTO
    {
        id_organe = o.id_organe,
        code_organe = o.code_organe,
        libelle_organe = o.libelle_organe,
        modele = o.modele,
        id_marque = o.id_marque,
        nom_marque = o.Marque.nom_fabriquant,
        caracteristiques = o.OrganeCaracteristiques
            .Select(oc => new OrganeCaracteristiqueDTO
            {
                id_organe = oc.id_organe,
                id_caracteristique = oc.id_caracteristique,
                valeur = oc.valeur,
                nomCaracteristique = oc.Caracteristique.libelle
            }).ToList()
    }).ToListAsync();
}

        public async Task<OrganeDTO?> GetByIdAsync(int id)
        {
            var o = await _context.Organes
                .Include(o => o.Marque)
                .Include(o => o.OrganeCaracteristiques)
                    .ThenInclude(oc => oc.Caracteristique)
                .FirstOrDefaultAsync(o => o.id_organe == id);

            if (o == null) return null;

            return new OrganeDTO
            {
                id_organe = o.id_organe,
                code_organe = o.code_organe,
                libelle_organe = o.libelle_organe,
                modele = o.modele,
                id_marque = o.id_marque,
                nom_marque = o.Marque.nom_fabriquant,
                caracteristiques = o.OrganeCaracteristiques
                    .Select(oc => new OrganeCaracteristiqueDTO
                    {
                        id_organe = oc.id_organe,
                        id_caracteristique = oc.id_caracteristique,
                        valeur = oc.valeur,
                        nomCaracteristique = oc.Caracteristique.libelle
                    }).ToList()
            };
        }

        public async Task<OrganeDTO> CreateAsync(CreateOrganeDTO dto)
        {
            // 🔵 Générer automatiquement le code organe
            string newCode = await GenerateNewCodeAsync();

            var organe = new Organe
            {
                code_organe = newCode, // auto-généré ici
                libelle_organe = dto.libelle_organe,
                modele = dto.modele,
                id_marque = dto.id_marque,
                OrganeCaracteristiques = dto.caracteristiques.Select(c => new OrganeCaracteristique
                {
                    id_caracteristique = c.id_caracteristique,
                    valeur = c.valeur
                }).ToList()
            };

            _context.Organes.Add(organe);
            await _context.SaveChangesAsync();

            // Load the related data before returning
            await _context.Entry(organe)
                .Reference(o => o.Marque)
                .LoadAsync();

            await _context.Entry(organe)
                .Collection(o => o.OrganeCaracteristiques)
                .Query()
                .Include(oc => oc.Caracteristique)
                .LoadAsync();

            return new OrganeDTO
            {
                id_organe = organe.id_organe,
                code_organe = organe.code_organe,
                libelle_organe = organe.libelle_organe,
                modele = organe.modele,
                id_marque = organe.id_marque,
                nom_marque = organe.Marque.nom_fabriquant,
                caracteristiques = organe.OrganeCaracteristiques
                    .Select(oc => new OrganeCaracteristiqueDTO
                    {
                        id_organe = oc.id_organe,
                        id_caracteristique = oc.id_caracteristique,
                        valeur = oc.valeur,
                        nomCaracteristique = oc.Caracteristique.libelle
                    }).ToList()
            };
        }

        public async Task<OrganeDTO?> UpdateAsync(int id, UpdateOrganeDTO dto)
        {
            var o = await _context.Organes
                .Include(o => o.OrganeCaracteristiques)
                .FirstOrDefaultAsync(o => o.id_organe == id);

            if (o == null) return null;

            o.libelle_organe = dto.libelle_organe;
            o.modele = dto.modele;
            o.id_marque = dto.id_marque;

            // Suppression des anciennes relations
            _context.OrganeCaracteristiques.RemoveRange(o.OrganeCaracteristiques);

            // Ajout des nouvelles
           o.OrganeCaracteristiques = dto.caracteristiques.Select(c => new OrganeCaracteristique{
            id_caracteristique = c.id_caracteristique,
            valeur = c.valeur,
            id_organe = o.id_organe
            }).ToList();

            await _context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        public async Task<bool> CanDeleteAsync(int id)
        {
            return !await _context.OrganeEquipements.AnyAsync(oe => oe.idorg == id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!await CanDeleteAsync(id)) return false;

            var o = await _context.Organes
                .Include(o => o.OrganeCaracteristiques)
                .FirstOrDefaultAsync(o => o.id_organe == id);

            if (o == null) return false;

            _context.OrganeCaracteristiques.RemoveRange(o.OrganeCaracteristiques);
            _context.Organes.Remove(o);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<string> GenerateNewCodeAsync()
{
    var lastOrgane = await _context.Organes
        .OrderByDescending(o => o.id_organe)
        .FirstOrDefaultAsync();

    int nextNumber = 1;

    if (lastOrgane != null && !string.IsNullOrEmpty(lastOrgane.code_organe))
    {
        // Supposons que le code est du style "ORG001", "ORG002" etc.
        string numberPart = new string(lastOrgane.code_organe.SkipWhile(c => !char.IsDigit(c)).ToArray());
        if (int.TryParse(numberPart, out int lastNumber))
        {
            nextNumber = lastNumber + 1;
        }
    }

    return $"ORG{nextNumber:D3}"; // Format : ORG001, ORG002, ORG003, etc.
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
        
        public async Task<int> GetOrganeCountAsync()
        {
            return await _context.Organes.CountAsync();
        }

     public async Task<IEnumerable<OrganeLightDTO>> GetOrganesByTypeAndMarqueAsync(int typeId, int marqueId)
{
    var organes = await _context.OrganeEquipements
        .Where(oe => oe.Equipement!.idType == typeId && oe.Equipement.idMarq == marqueId)
        .Select(oe => new OrganeLightDTO
        {
            id_organe = oe.Organe!.id_organe,
            libelle_organe = oe.Organe.libelle_organe
        })
        .Distinct()
        .ToListAsync();

    return organes;
}
    }
}