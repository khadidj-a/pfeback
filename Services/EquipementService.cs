using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFE_PROJECT.Services
{
    public class EquipementService : IEquipementService
    {
        private readonly ApplicationDbContext _context;

        public EquipementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EquipementDTO>> GetAllAsync(EquipementFilterDTO? filter = null)
        {
            var query = _context.Equipements
                .Include(e => e.TypeEquip)
                .Include(e => e.Categorie)
                .Include(e => e.Marque)
                .Include(e => e.GroupeIdentique)
                .Include(e => e.Unite)
                .AsQueryable();

            if (filter != null)
            {
                // Apply filters
                if (filter.idCat.HasValue)
                    query = query.Where(e => e.idCat == filter.idCat);
                
                if (!string.IsNullOrEmpty(filter.état))
                    query = query.Where(e => e.état == filter.état);
                
                if (filter.idMarq.HasValue)
                    query = query.Where(e => e.idMarq == filter.idMarq);
                
                if (filter.idType.HasValue)
                    query = query.Where(e => e.idType == filter.idType);
                
                if (filter.idGrpIdq.HasValue)
                    query = query.Where(e => e.idGrpIdq == filter.idGrpIdq);
                
                if (filter.DateMiseService.HasValue)
                    query = query.Where(e => e.DateMiseService == filter.DateMiseService);
                
                if (filter.AnnéeFabrication.HasValue)
                    query = query.Where(e => e.AnnéeFabrication == filter.AnnéeFabrication);
                
                if (filter.DateAcquisition.HasValue)
                    query = query.Where(e => e.DateAcquisition == filter.DateAcquisition);
                
                if (filter.ValeurAcquisition.HasValue)
                    query = query.Where(e => e.ValeurAcquisition == filter.ValeurAcquisition);

                // Apply search term
                if (!string.IsNullOrEmpty(filter.searchTerm))
                {
                    string lowerSearch = filter.searchTerm.ToLower();
                    query = query.Where(e =>
                        e.design.ToLower().Contains(lowerSearch) ||
                        e.codeEqp.ToLower().Contains(lowerSearch));
                }

                // Apply sorting
                query = (filter.sortBy?.ToLower() ?? "idEqpt") switch
                {
                    "design" => filter.ascending ? query.OrderBy(e => e.design) : query.OrderByDescending(e => e.design),
                    "codeeqp" => filter.ascending ? query.OrderBy(e => e.codeEqp) : query.OrderByDescending(e => e.codeEqp),
                    "etat" => filter.ascending ? query.OrderBy(e => e.état) : query.OrderByDescending(e => e.état),
                    _ => filter.ascending ? query.OrderBy(e => e.idEqpt) : query.OrderByDescending(e => e.idEqpt)
                };
            }

            return await query.Select(e => new EquipementDTO
            {
                idEqpt = e.idEqpt,
                idType = e.idType,
                typeDesignation = e.TypeEquip.designation,
                idCat = e.idCat,
                categorieDesignation = e.Categorie.designation,
                idMarq = e.idMarq,
                marqueNom = e.Marque.nom_fabriquant,
                codeEqp = e.codeEqp,
                design = e.design,
                idGrpIdq = e.idGrpIdq,
                groupeIdentiqueDesignation = e.GroupeIdentique != null ? e.GroupeIdentique.codegrp : null,
                état = e.état,
                DateMiseService = e.DateMiseService,
                AnnéeFabrication = e.AnnéeFabrication,
                DateAcquisition = e.DateAcquisition,
                ValeurAcquisition = e.ValeurAcquisition,
                idunite = e.idunite,
                uniteDesignation = e.Unite != null ? e.Unite.designation : null
            }).ToListAsync();
        }

        public async Task<EquipementDTO?> GetByIdAsync(int id)
        {
            var equipement = await _context.Equipements
                .Include(e => e.TypeEquip)
                .Include(e => e.Categorie)
                .Include(e => e.Marque)
                .Include(e => e.GroupeIdentique)
                .Include(e => e.Unite)
                .FirstOrDefaultAsync(e => e.idEqpt == id);

            if (equipement == null) return null;

            return new EquipementDTO
            {
                idEqpt = equipement.idEqpt,
                idType = equipement.idType,
                typeDesignation = equipement.TypeEquip.designation,
                idCat = equipement.idCat,
                categorieDesignation = equipement.Categorie.designation,
                idMarq = equipement.idMarq,
                marqueNom = equipement.Marque.nom_fabriquant,
                codeEqp = equipement.codeEqp,
                design = equipement.design,
                idGrpIdq = equipement.idGrpIdq,
                groupeIdentiqueDesignation = equipement.GroupeIdentique != null ? equipement.GroupeIdentique.codegrp : null,
                état = equipement.état,
                DateMiseService = equipement.DateMiseService,
                AnnéeFabrication = equipement.AnnéeFabrication,
                DateAcquisition = equipement.DateAcquisition,
                ValeurAcquisition = equipement.ValeurAcquisition,
                idunite = equipement.idunite,
                uniteDesignation = equipement.Unite != null ? equipement.Unite.designation : null
            };
        }

        public async Task<EquipementDTO?> GetByCodeAsync(string code)
        {
            var equipement = await _context.Equipements
                .Include(e => e.TypeEquip)
                .Include(e => e.Categorie)
                .Include(e => e.Marque)
                .Include(e => e.GroupeIdentique)
                .Include(e => e.Unite)
                .FirstOrDefaultAsync(e => e.codeEqp == code);

            if (equipement == null) return null;

            return new EquipementDTO
            {
                idEqpt = equipement.idEqpt,
                idType = equipement.idType,
                typeDesignation = equipement.TypeEquip.designation,
                idCat = equipement.idCat,
                categorieDesignation = equipement.Categorie.designation,
                idMarq = equipement.idMarq,
                marqueNom = equipement.Marque.nom_fabriquant,
                codeEqp = equipement.codeEqp,
                design = equipement.design,
                idGrpIdq = equipement.idGrpIdq,
                groupeIdentiqueDesignation = equipement.GroupeIdentique != null ? equipement.GroupeIdentique.codegrp : null,
                état = equipement.état,
                DateMiseService = equipement.DateMiseService,
                AnnéeFabrication = equipement.AnnéeFabrication,
                DateAcquisition = equipement.DateAcquisition,
                ValeurAcquisition = equipement.ValeurAcquisition,
                idunite = equipement.idunite,
                uniteDesignation = equipement.Unite != null ? equipement.Unite.designation : null
            };
        }

        public async Task<EquipementDTO> CreateAsync(CreateEquipementDTO dto)
        {
            // Validate état
            if (!await ValidateEtatAsync(dto.état))
                throw new ArgumentException("État invalide");
            
            // Get the categorie from database to validate
            var categorie = await _context.Categories.FindAsync(dto.idCat);
            if (categorie == null)
                throw new ArgumentException("Catégorie invalide");

            // Get the marque to get its code
            var marque = await _context.Marques.FindAsync(dto.idMarq);
            if (marque == null)
                throw new ArgumentException("Marque invalide");

            // Find or create GroupeIdentique
            int? idGrpIdq = null;
            if (dto.idGrpIdq == null)
            {
                // Look for existing group with same Marque and TypeEquip
                var existingGroup = await _context.GroupeIdentiques
                    .FirstOrDefaultAsync(g => g.id_marque == dto.idMarq && g.id_type_equip == dto.idType);

                if (existingGroup != null)
                {
                    idGrpIdq = existingGroup.Id;
                }
                else
                {
                    // Create new group
                    var newGroup = new GroupeIdentique
                    {
                        id_marque = dto.idMarq,
                        id_type_equip = dto.idType,
                        codegrp = $"{marque.codemarque}{dto.idType}"
                    };
                    _context.GroupeIdentiques.Add(newGroup);
                    await _context.SaveChangesAsync();
                    idGrpIdq = newGroup.Id;
                }
            }
            else
            {
                // Use provided group if it exists
                var providedGroup = await _context.GroupeIdentiques.FindAsync(dto.idGrpIdq);
                if (providedGroup == null)
                    throw new ArgumentException("GroupeIdentique invalide");
                
                // Verify that the provided group matches the Marque and TypeEquip
                if (providedGroup.id_marque != dto.idMarq || providedGroup.id_type_equip != dto.idType)
                    throw new ArgumentException("Le GroupeIdentique fourni ne correspond pas à la Marque et au TypeEquip");
                
                idGrpIdq = dto.idGrpIdq;
            }

            var equipement = new Equipement
            {
                idType = dto.idType,
                idCat = dto.idCat,
                idMarq = dto.idMarq,
                design = dto.design,
                idGrpIdq = idGrpIdq,
                état = dto.état,
                DateMiseService = dto.DateMiseService,
                AnnéeFabrication = dto.AnnéeFabrication,
                DateAcquisition = dto.DateAcquisition,
                ValeurAcquisition = dto.ValeurAcquisition,
                idunite = dto.idunite
            };

            _context.Equipements.Add(equipement);
            await _context.SaveChangesAsync();

            // Generate codeEqp after we have the idEqpt
            if (marque.codemarque == null)
                throw new ArgumentException("Le code de la marque est null");

            equipement.codeEqp = 
                equipement.design.Substring(0, Math.Min(2, equipement.design.Length)).ToUpper() +
                marque.codemarque.Substring(0, Math.Min(3, marque.codemarque.Length)).ToUpper() +
                equipement.idEqpt.ToString().PadLeft(3, '0');

            await _context.SaveChangesAsync();

            return await GetByIdAsync(equipement.idEqpt) ?? throw new Exception("Création échouée");
        }

        public async Task<EquipementDTO?> UpdateAsync(int id, UpdateEquipementDTO dto)
        {
            var equipement = await _context.Equipements.FindAsync(id);
            if (equipement == null) return null;

            // Validate état
            if (!await ValidateEtatAsync(dto.état))
                throw new ArgumentException("État invalide");
            
            // Get the categorie from database to validate
            var categorie = await _context.Categories.FindAsync(dto.idCat);
            if (categorie == null)
                throw new ArgumentException("Catégorie invalide");

            equipement.idType = dto.idType;
            equipement.idCat = dto.idCat;
            equipement.idMarq = dto.idMarq;
            equipement.design = dto.design;
            equipement.idGrpIdq = dto.idGrpIdq;
            equipement.état = dto.état;
            equipement.DateMiseService = dto.DateMiseService;
            equipement.AnnéeFabrication = dto.AnnéeFabrication;
            equipement.DateAcquisition = dto.DateAcquisition;
            equipement.ValeurAcquisition = dto.ValeurAcquisition;
            equipement.idunite = dto.idunite;

            await _context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var equipement = await _context.Equipements.FindAsync(id);
            if (equipement == null) return false;

            _context.Equipements.Remove(equipement);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ValidateEtatAsync(string? état)
        {
            if (string.IsNullOrEmpty(état)) return true;
            
            var validEtats = new[] { "Prêt", "En stock", "Réformé", "En panne", "En Service" };
            return validEtats.Contains(état);
        }

        public async Task<bool> ValidateCategorieAsync(string? categorie)
        {
            if (string.IsNullOrEmpty(categorie)) return true;
            
            var validCategories = new[] { "Soutien", "Fixes", "Roulants" };
            return validCategories.Contains(categorie);
        }
    }
} 