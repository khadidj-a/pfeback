

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
            try
            {
                Console.WriteLine($"🔍 Recherche: {filter?.searchTerm ?? "aucune"} | Tri: {filter?.sortBy ?? "idEqpt"} | Ordre: {(filter?.ascending ?? true ? "Ascendant" : "Descendant")}");

                // Start with base query using explicit joins
                var query = from e in _context.Equipements
                           join t in _context.TypeEquips on e.idType equals t.idtypequip into typeJoin
                           from type in typeJoin.DefaultIfEmpty()
                           join c in _context.Categories on e.idCat equals c.idcategorie into catJoin
                           from cat in catJoin.DefaultIfEmpty()
                           join m in _context.Marques on e.idMarq equals m.idmarque into marqJoin
                           from marq in marqJoin.DefaultIfEmpty()
                           join g in _context.GroupeIdentiques on e.idGrpIdq equals g.Id into grpJoin
                           from grp in grpJoin.DefaultIfEmpty()
                           join u in _context.Unites on e.idunite equals u.idunite into uniteJoin
                           from unite in uniteJoin.DefaultIfEmpty()
                           select new EquipementDTO
                           {
                               idEqpt = e.idEqpt,
                               idType = e.idType,
                               typeDesignation = type != null ? type.designation : string.Empty,
                               idCat = e.idCat,
                               categorieDesignation = cat != null ? cat.designation : string.Empty,
                               idMarq = e.idMarq,
                               marqueNom = marq != null ? marq.nom_fabriquant : string.Empty,
                               codeEqp = e.codeEqp ?? string.Empty,
                               design = e.design ?? string.Empty,
                               idGrpIdq = e.idGrpIdq,
                               groupeIdentiqueDesignation = grp != null ? grp.codegrp : string.Empty,
                               etat = e.etat ?? string.Empty,
                               numserie = e.numserie,
                               position_physique = e.position_physique,
                               DateMiseService = e.DateMiseService,
                               AnnéeFabrication = e.AnnéeFabrication,
                               DateAcquisition = e.DateAcquisition,
                               ValeurAcquisition = e.ValeurAcquisition,
                               observation = e.observation,
                               idunite = e.idunite,
                               uniteDesignation = unite != null ? unite.designation : string.Empty
                           };

                // Apply search term if provided
                if (filter != null && !string.IsNullOrEmpty(filter.searchTerm))
                {
                    var searchTerm = filter.searchTerm.ToLower().Trim();
                    query = query.Where(e =>
                        (e.design != null && e.design.ToLower().Contains(searchTerm)) ||
                        (e.codeEqp != null && e.codeEqp.ToLower().Contains(searchTerm)) ||
                        (e.numserie != null && e.numserie.ToLower().Contains(searchTerm)) ||
                        (e.position_physique != null && e.position_physique.ToLower().Contains(searchTerm)) ||
                        (e.typeDesignation != null && e.typeDesignation.ToLower().Contains(searchTerm)) ||
                        (e.marqueNom != null && e.marqueNom.ToLower().Contains(searchTerm)) ||
                        (e.categorieDesignation != null && e.categorieDesignation.ToLower().Contains(searchTerm)) ||
                        (e.uniteDesignation != null && e.uniteDesignation.ToLower().Contains(searchTerm)) ||
                        (e.etat != null && e.etat.ToLower().Contains(searchTerm))
                    );
                }

                // Apply filters if provided
                if (filter != null)
                {
                    // Apply flexible filters
                    if (filter.idCat.HasValue && filter.idCat > 0)
                        query = query.Where(e => e.idCat == filter.idCat);
                    
                    if (!string.IsNullOrEmpty(filter.etat))
                        query = query.Where(e => e.etat != null && e.etat.ToLower().Contains(filter.etat.ToLower()));
                    
                    if (filter.idMarq.HasValue && filter.idMarq > 0)
                        query = query.Where(e => e.idMarq == filter.idMarq);
                    
                    if (filter.idType.HasValue && filter.idType > 0)
                        query = query.Where(e => e.idType == filter.idType);
                    
                    if (filter.idGrpIdq.HasValue && filter.idGrpIdq > 0)
                        query = query.Where(e => e.idGrpIdq == filter.idGrpIdq);
                    
                    if (filter.idunite.HasValue && filter.idunite > 0)
                        query = query.Where(e => e.idunite == filter.idunite);
                    
                    if (!string.IsNullOrEmpty(filter.numserie))
                        query = query.Where(e => e.numserie != null && e.numserie.ToLower().Contains(filter.numserie.ToLower()));
                    
                    if (!string.IsNullOrEmpty(filter.position_physique))
                        query = query.Where(e => e.position_physique != null && e.position_physique.ToLower().Contains(filter.position_physique.ToLower()));
                    
                    if (!string.IsNullOrEmpty(filter.design))
                        query = query.Where(e => e.design != null && e.design.ToLower().Contains(filter.design.ToLower()));
                    
                    if (filter.DateMiseService.HasValue)
                        query = query.Where(e => e.DateMiseService.HasValue && 
                            e.DateMiseService.Value.Date == filter.DateMiseService.Value.Date);
                    
                    if (filter.AnnéeFabrication.HasValue)
                        query = query.Where(e => e.AnnéeFabrication == filter.AnnéeFabrication);
                    
                    if (filter.DateAcquisition.HasValue)
                        query = query.Where(e => e.DateAcquisition.HasValue && 
                            e.DateAcquisition.Value.Date == filter.DateAcquisition.Value.Date);
                    
                    if (filter.ValeurAcquisition.HasValue)
                        query = query.Where(e => e.ValeurAcquisition == filter.ValeurAcquisition);

                    // Apply sorting
                    var sortBy = filter.sortBy?.ToLower() ?? "idEqpt";
                    query = sortBy switch
                    {
                        "design" => filter.ascending ? query.OrderBy(e => e.design) : query.OrderByDescending(e => e.design),
                        "codeeqp" => filter.ascending ? query.OrderBy(e => e.codeEqp) : query.OrderByDescending(e => e.codeEqp),
                        "etat" => filter.ascending ? query.OrderBy(e => e.etat) : query.OrderByDescending(e => e.etat),
                        "numserie" => filter.ascending ? query.OrderBy(e => e.numserie) : query.OrderByDescending(e => e.numserie),
                        "position_physique" => filter.ascending ? query.OrderBy(e => e.position_physique) : query.OrderByDescending(e => e.position_physique),
                        "dateacquisition" => filter.ascending ? query.OrderBy(e => e.DateAcquisition) : query.OrderByDescending(e => e.DateAcquisition),
                        "datemiseservice" => filter.ascending ? query.OrderBy(e => e.DateMiseService) : query.OrderByDescending(e => e.DateMiseService),
                        _ => filter.ascending ? query.OrderBy(e => e.idEqpt) : query.OrderByDescending(e => e.idEqpt)
                    };
                }
                else
                {
                    // Default sorting when no filter is provided
                    query = query.OrderBy(e => e.idEqpt);
                }

                // Execute the query
                var results = await query.ToListAsync();
                Console.WriteLine($"�� Résultats trouvés: {results.Count}");
                return results;
            }
            catch (Exception ex)
            {
                // Log the full exception details
                Console.WriteLine($"Error in GetAllAsync: {ex}");
                Console.WriteLine($"Inner Exception: {ex.InnerException}");
                throw new Exception($"Error retrieving equipment: {ex.Message}", ex);
            }
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
                observation = equipement.observation,
                idGrpIdq = equipement.idGrpIdq,
                groupeIdentiqueDesignation = equipement.GroupeIdentique != null ? equipement.GroupeIdentique.codegrp : null,
                etat = equipement.etat,
                numserie = equipement.numserie,
                position_physique = equipement.position_physique,
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
                observation = equipement.observation,
                idGrpIdq = equipement.idGrpIdq,
                groupeIdentiqueDesignation = equipement.GroupeIdentique != null ? equipement.GroupeIdentique.codegrp : null,
                etat = equipement.etat,
                numserie = equipement.numserie,
                position_physique = equipement.position_physique,
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
            // Validate etat
            if (!await ValidateEtatAsync(dto.etat))
                throw new ArgumentException("État invalide. Les états valides sont: operationnel, En panne, pre_reforme, reforme");
            
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
                observation = dto.observation ?? string.Empty,
                etat = dto.etat ?? "operationnel", // Default to operationnel if not provided
                numserie = dto.numserie,
                position_physique = dto.position_physique,
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
            var equipement = await _context.Equipements
                .Include(e => e.TypeEquip)
                .Include(e => e.Categorie)
                .Include(e => e.Marque)
                .Include(e => e.GroupeIdentique)
                .Include(e => e.Unite)
                .FirstOrDefaultAsync(e => e.idEqpt == id);

            if (equipement == null) return null;

            // Check if equipment is in a restricted state
            if (equipement.etat == "reforme" || equipement.etat == "pre_reforme")
            {
                throw new InvalidOperationException($"Impossible de modifier un équipement en état '{equipement.etat}'. Les équipements en état 'pre_reforme' ou 'reforme' ne peuvent pas être modifiés.");
            }

            // Validate etat if provided and not empty
            if (!string.IsNullOrEmpty(dto.etat) && dto.etat != "string" && !await ValidateEtatAsync(dto.etat))
                throw new ArgumentException("État invalide. Les états valides sont: operationnel, En panne, pre_reforme, reforme");

            // Update fields without restrictive conditions
            if (dto.idType > 0) equipement.idType = dto.idType;
            if (dto.idCat > 0) equipement.idCat = dto.idCat;
            if (dto.idMarq > 0) equipement.idMarq = dto.idMarq;
            if (!string.IsNullOrEmpty(dto.design) && dto.design != "string") equipement.design = dto.design;
            if (dto.idGrpIdq.HasValue) equipement.idGrpIdq = dto.idGrpIdq;
            if (!string.IsNullOrEmpty(dto.etat) && dto.etat != "string") equipement.etat = dto.etat;
            if (!string.IsNullOrEmpty(dto.numserie)) equipement.numserie = dto.numserie;
            if (!string.IsNullOrEmpty(dto.position_physique)) equipement.position_physique = dto.position_physique;
            //ate observation to match create behavior
            equipement.observation = dto.observation ?? string.Empty;
            if (dto.DateMiseService.HasValue) equipement.DateMiseService = dto.DateMiseService;
            if (dto.AnnéeFabrication.HasValue) equipement.AnnéeFabrication = dto.AnnéeFabrication;
            if (dto.DateAcquisition.HasValue) equipement.DateAcquisition = dto.DateAcquisition;
            if (dto.ValeurAcquisition.HasValue) equipement.ValeurAcquisition = dto.ValeurAcquisition;
            if (dto.idunite.HasValue) equipement.idunite = dto.idunite;

            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine($"Équipement mis à jour avec succès: {equipement.idEqpt}");
                return await GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la mise à jour: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var equipement = await _context.Equipements.FindAsync(id);
            if (equipement == null) return false;

            _context.Equipements.Remove(equipement);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ValidateEtatAsync(string? etat)
        {
            if (string.IsNullOrEmpty(etat)) return true;
            
            var validEtats = new[] { "operationnel", "En panne", "pre_reforme", "reforme" };
            return validEtats.Contains(etat);
        }
        
  public async Task<IEnumerable<EquipementDTO>> GetNonReformedAsync()
{
    try
    {
        var equipements = await (from e in _context.Equipements
                                 where e.etat != "Reforme"
                                 join t in _context.TypeEquips on e.idType equals t.idtypequip into typeJoin
                                 from type in typeJoin.DefaultIfEmpty()
                                 join c in _context.Categories on e.idCat equals c.idcategorie into catJoin
                                 from cat in catJoin.DefaultIfEmpty()
                                 join m in _context.Marques on e.idMarq equals m.idmarque into marqJoin
                                 from marq in marqJoin.DefaultIfEmpty()
                                 join g in _context.GroupeIdentiques on e.idGrpIdq equals g.Id into grpJoin
                                 from grp in grpJoin.DefaultIfEmpty()
                                 join u in _context.Unites on e.idunite equals u.idunite into uniteJoin
                                 from unite in uniteJoin.DefaultIfEmpty()
                                 select new EquipementDTO
                                 {
                                     idEqpt = e.idEqpt,
                                     idType = e.idType,
                                     typeDesignation = type != null ? type.designation : string.Empty,
                                     idCat = e.idCat,
                                     categorieDesignation = cat != null ? cat.designation : string.Empty,
                                     idMarq = e.idMarq,
                                     marqueNom = marq != null ? marq.nom_fabriquant : string.Empty,
                                     codeEqp = e.codeEqp ?? string.Empty,
                                     design = e.design ?? string.Empty,
                                     idGrpIdq = e.idGrpIdq,
                                     groupeIdentiqueDesignation = grp != null ? grp.codegrp : string.Empty,
                                     etat = e.etat ?? string.Empty,
                                     DateMiseService = e.DateMiseService,
                                     AnnéeFabrication = e.AnnéeFabrication,
                                     DateAcquisition = e.DateAcquisition,
                                     ValeurAcquisition = e.ValeurAcquisition,
                                     idunite = e.idunite,
                uniteDesignation = e.Unite != null ? e.Unite.designation : null,
    position_physique = e.position_physique ?? string.Empty
                                 }).ToListAsync();

        return equipements;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur dans GetNonReformedEquipementsAsync: {ex.Message}");
        throw new Exception("Erreur lors de la récupération des équipements non reformés.", ex);
    }
}
    }
} 