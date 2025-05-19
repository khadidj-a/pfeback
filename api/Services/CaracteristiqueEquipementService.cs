   using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public class CaracteristiqueEquipementService : ICaracteristiqueEquipementService
    {
        private readonly ApplicationDbContext _context;

        public CaracteristiqueEquipementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CaracteristiqueEquipementDTO>> GetByEquipementIdAsync(int ideqpt, bool showValue = true)
        {
            return await _context.CaracteristiqueEquipements
                .Where(ce => ce.ideqpt == ideqpt)
                .Select(ce => new CaracteristiqueEquipementDTO
                {
                    ideqpt = ce.ideqpt,
                    idcarac = ce.idcarac,
                    valeur = showValue ? ce.valeur : string.Empty,
                    nomcarac = ce.Caracteristique!.libelle
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CaracteristiqueEquipementDTO>> GetByTypeAndMarqueAsync(int typeId, int marqueId)
        {
            return await _context.CaracteristiqueEquipements
                .Where(ce => ce.Equipement!.idType == typeId && ce.Equipement.idMarq == marqueId)
                .Select(ce => new CaracteristiqueEquipementDTO
                {
                    ideqpt = ce.ideqpt,
                    idcarac = ce.idcarac,
                    valeur = ce.valeur,
                    nomcarac = ce.Caracteristique!.libelle
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CaracteristiqueEquipementDTO>> CreateAsync(BulkCreateCaracteristiqueEquipementDTO dto)
        {
            var caracteristiqueEquipements = dto.Caracteristiques.Select(c => new CaracteristiqueEquipement
            {
                ideqpt = dto.ideqpt,
                idcarac = c.idcarac,
                valeur = c.valeur
            }).ToList();

            _context.CaracteristiqueEquipements.AddRange(caracteristiqueEquipements);
            await _context.SaveChangesAsync();

            // Get the equipment to find its groupe ID
            var equipement = await _context.Equipements
                .FirstOrDefaultAsync(e => e.idEqpt == dto.ideqpt);

            if (equipement != null)
            {
                // Create entries in GroupeCaracteristique
                var groupeCaracteristiques = dto.Caracteristiques.Select(c => new GroupeCaracteristique
                {
                    idgrpidq = equipement.idGrpIdq ?? 0,
                    idcarac = c.idcarac
                }).ToList();

                _context.GroupeCaracteristiques.AddRange(groupeCaracteristiques);
                await _context.SaveChangesAsync();
            }

            return await GetByEquipementIdAsync(dto.ideqpt);
        }

      public async Task<IEnumerable<CaracteristiqueEquipementDTO>> BulkCreateAsync(BulkCreateCaracteristiqueEquipementDTO dto)
{
    // Récupère les idcarac déjà existants pour cet équipement
    var existingCaracs = await _context.CaracteristiqueEquipements
        .Where(ce => ce.ideqpt == dto.ideqpt)
        .Select(ce => ce.idcarac)
        .ToListAsync();

    // Filtre les nouvelles caractéristiques à ajouter
    var newCaracteristiques = dto.Caracteristiques
        .Where(c => !existingCaracs.Contains(c.idcarac))
        .ToList();

    // Crée les entités à insérer
    var caracteristiqueEquipements = newCaracteristiques.Select(c => new CaracteristiqueEquipement
    {
        ideqpt = dto.ideqpt,
        idcarac = c.idcarac,
        valeur = c.valeur
    }).ToList();

    _context.CaracteristiqueEquipements.AddRange(caracteristiqueEquipements);
    await _context.SaveChangesAsync();

    // Ajout dans GroupeCaracteristique si l'équipement a un groupe
    var equipement = await _context.Equipements
        .FirstOrDefaultAsync(e => e.idEqpt == dto.ideqpt);

    if (equipement != null && equipement.idGrpIdq != null)
    {
        var existingGroupeCaracs = await _context.GroupeCaracteristiques
            .Where(gc => gc.idgrpidq == equipement.idGrpIdq)
            .Select(gc => gc.idcarac)
            .ToListAsync();

        var newGroupeCaracs = newCaracteristiques
            .Where(c => !existingGroupeCaracs.Contains(c.idcarac))
            .Select(c => new GroupeCaracteristique
            {
                idgrpidq = equipement.idGrpIdq.Value,
                idcarac = c.idcarac
            }).ToList();

        _context.GroupeCaracteristiques.AddRange(newGroupeCaracs);
        await _context.SaveChangesAsync();
    }

    // Retourner les caractéristiques complètes avec les noms
    return await GetByEquipementIdAsync(dto.ideqpt);
}



        public async Task<CaracteristiqueEquipementDTO?> UpdateAsync(int ideqpt, int idcarac, UpdateCaracteristiqueEquipementDTO dto)
        {
            var caracteristiqueEquipement = await _context.CaracteristiqueEquipements
                .Include(ce => ce.Caracteristique)
                .FirstOrDefaultAsync(ce => ce.ideqpt == ideqpt && ce.idcarac == idcarac);

            if (caracteristiqueEquipement == null)
                return null;

            caracteristiqueEquipement.valeur = dto.valeur;
            await _context.SaveChangesAsync();

            return new CaracteristiqueEquipementDTO
            {
                ideqpt = caracteristiqueEquipement.ideqpt,
                idcarac = caracteristiqueEquipement.idcarac,
                valeur = caracteristiqueEquipement.valeur,
                nomcarac = caracteristiqueEquipement.Caracteristique?.libelle ?? string.Empty
            };
        }
    }
} 

