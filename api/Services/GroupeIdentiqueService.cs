using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;

public class GroupeIdentiqueService : IGroupeIdentiqueService
{
    private readonly ApplicationDbContext _context;

    public GroupeIdentiqueService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GroupeIdentiqueDTO>> GetAllAsync(
    string? searchTerm = null,
    string? sortBy = null,
    bool ascending = true)
    {
        var query = _context.GroupeIdentiques
            .Include(g => g.Marque)
            .Include(g => g.TypeEquip)
            .Include(g => g.GroupeOrganes).ThenInclude(go => go.Organe)
            .Include(g => g.GroupeCaracteristiques).ThenInclude(gc => gc.Caracteristique)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            string lowerSearch = searchTerm.ToLower();
            query = query.Where(g =>
                g.codegrp.ToLower().Contains(lowerSearch) ||
                g.Marque.nom_fabriquant.ToLower().Contains(lowerSearch) ||
                g.TypeEquip.designation.ToLower().Contains(lowerSearch));
        }

        sortBy = sortBy?.ToLower() ?? "id";

        query = sortBy switch
        {
            "codegrp" => ascending ? query.OrderBy(g => g.codegrp) : query.OrderByDescending(g => g.codegrp),
            "marque" => ascending ? query.OrderBy(g => g.Marque.nom_fabriquant) : query.OrderByDescending(g => g.Marque.nom_fabriquant),
            "type" or "typeequip" => ascending ? query.OrderBy(g => g.TypeEquip.designation) : query.OrderByDescending(g => g.TypeEquip.designation),
            _ => ascending ? query.OrderBy(g => g.Id) : query.OrderByDescending(g => g.Id)
        };

        return await query.Select(g => new GroupeIdentiqueDTO
        {
            Id = g.Id,
            CodeGrp = g.codegrp,
            MarqueNom = g.Marque.nom_fabriquant,
            TypeEquipNom = g.TypeEquip.designation,
            Organes = g.GroupeOrganes.Select(o => o.Organe.libelle_organe).ToList(),
            Caracteristiques = g.GroupeCaracteristiques.Select(c => c.Caracteristique.libelle).ToList()
        }).ToListAsync();
    }

   public async Task<GroupeIdentiqueDTO?> GetByIdAsync(int id)
{
    var g = await _context.GroupeIdentiques
        .Include(g => g.Marque)
        .Include(g => g.TypeEquip)
        .Include(g => g.GroupeOrganes).ThenInclude(go => go.Organe)
        .Include(g => g.GroupeCaracteristiques).ThenInclude(gc => gc.Caracteristique)
        .FirstOrDefaultAsync(g => g.Id == id);

    if (g == null) return null;
return new GroupeIdentiqueDTO
{
    Id = g.Id,
    CodeGrp = g.codegrp,
    MarqueNom = g.Marque.nom_fabriquant,
    TypeEquipNom = g.TypeEquip.designation,
    IdMarque = g.id_marque,
    IdType = g.id_type_equip,

    Organes = g.GroupeOrganes.Select(o => o.Organe.libelle_organe).ToList(),
    OrganesIds = g.GroupeOrganes.Select(o => o.Organe.id_organe).ToList(), // 👈
    Caracteristiques = g.GroupeCaracteristiques.Select(c => c.Caracteristique.libelle).ToList(),
    CaracteristiquesIds = g.GroupeCaracteristiques.Select(c => c.Caracteristique.id_caracteristique).ToList() // 👈
};

}


    public async Task<GroupeIdentiqueDTO> CreateAsync(CreateGroupeIdentiqueDTO dto)
    {
        var marque = await _context.Marques.FindAsync(dto.id_marque);
        var typeEquip = await _context.TypeEquips.FindAsync(dto.id_type_equip);

        if (marque == null || typeEquip == null)
            throw new ArgumentException("Marque or TypeEquip not found");

        var groupe = new GroupeIdentique
        {
            id_marque = dto.id_marque,
            id_type_equip = dto.id_type_equip,
            codegrp = $"{marque.codemarque}{typeEquip.codetype}"
        };

        _context.GroupeIdentiques.Add(groupe);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(groupe.Id);
    }

    public async Task<GroupeIdentiqueDTO?> UpdateAsync(int id, UpdateGroupeIdentiqueDTO dto)
    {
        var groupe = await _context.GroupeIdentiques
            .Include(g => g.GroupeOrganes)
            .Include(g => g.GroupeCaracteristiques)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (groupe == null) return null;

        _context.GroupeOrganes.RemoveRange(groupe.GroupeOrganes);
        _context.GroupeCaracteristiques.RemoveRange(groupe.GroupeCaracteristiques);

        groupe.GroupeOrganes = dto.id_organes.Select(orgId => new GroupeOrgane
        {
            idgrpidq = id,
            idorg = orgId
        }).ToList();

        groupe.GroupeCaracteristiques = dto.id_caracteristiques.Select(caracId => new GroupeCaracteristique
        {
            idgrpidq = id,
            idcarac = caracId
        }).ToList();

        await _context.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

 public async Task<bool> DeleteAsync(int id)
    {
        if (!await CanDeleteGroupeAsync(id)) return false;

        var group = await _context.GroupeIdentiques.FindAsync(id);
        if (group == null) return false;

        _context.GroupeIdentiques.Remove(group);
        await _context.SaveChangesAsync();
        return true;
    }
        public async Task<bool> CanDeleteGroupeAsync(int id)
        {
            return !await _context.Equipements.AnyAsync(e => e.idGrpIdq == id);
        }

    public async Task<GroupeDetailsDTO?> GetGroupDetailsAsync(int id)
    {
        var groupe = await _context.GroupeIdentiques
            .Include(g => g.Marque)
            .Include(g => g.TypeEquip)
            .Include(g => g.GroupeOrganes)
                .ThenInclude(go => go.Organe)
            .Include(g => g.GroupeCaracteristiques)
                .ThenInclude(gc => gc.Caracteristique)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (groupe == null) return null;

        return new GroupeDetailsDTO
        {
            Id = groupe.Id,
            CodeGrp = groupe.codegrp,
            MarqueNom = groupe.Marque.nom_fabriquant,
            TypeEquipNom = groupe.TypeEquip.designation,
            Organes = groupe.GroupeOrganes.Select(go => new OrganeDetailDTO
            {
                idorg = go.Organe.id_organe,
                libelle_organe = go.Organe.libelle_organe
            }).ToList(),
            Caracteristiques = groupe.GroupeCaracteristiques.Select(gc => new CaracteristiqueDetailDTO
            {
                idcarac = gc.Caracteristique.id_caracteristique,
                libelle = gc.Caracteristique.libelle
            }).ToList()
        };
    }
      public async Task<int> GetGroupeCountAsync()
{
    return await _context.GroupeIdentiques.CountAsync();
}

 public async Task<IEnumerable<GroupeIdentiqueDTO>> GetByTypeAndMarqueAsync(int typeId, int marqueId)
    {
        return await _context.GroupeIdentiques
            .Include(g => g.Marque)
            .Include(g => g.TypeEquip)
            .Include(g => g.GroupeOrganes).ThenInclude(go => go.Organe)
            .Include(g => g.GroupeCaracteristiques).ThenInclude(gc => gc.Caracteristique)
            .Where(g => g.id_type_equip == typeId && g.id_marque == marqueId)
            .Select(g => new GroupeIdentiqueDTO
            {
                Id = g.Id,
                CodeGrp = g.codegrp,
                MarqueNom = g.Marque.nom_fabriquant,
                TypeEquipNom = g.TypeEquip.designation,
                IdMarque = g.id_marque,
                IdType = g.id_type_equip,
                Organes = g.GroupeOrganes.Select(o => o.Organe.libelle_organe).ToList(),
                OrganesIds = g.GroupeOrganes.Select(o => o.Organe.id_organe).ToList(),
                Caracteristiques = g.GroupeCaracteristiques.Select(c => c.Caracteristique.libelle).ToList(),
                CaracteristiquesIds = g.GroupeCaracteristiques.Select(c => c.Caracteristique.id_caracteristique).ToList()
            })
            .ToListAsync();
    }
}
