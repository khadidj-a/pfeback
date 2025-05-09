using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public class AffectationService : IAffectationService
    {
        private readonly ApplicationDbContext _context;

        public AffectationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AffectationDTO>> GetAllAsync(string? searchTerm = null, string? sortBy = null, bool ascending = true)
        {
            var query = _context.Affectations
                .Include(a => a.Equipement)
                .Include(a => a.Unite)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string lowerSearch = searchTerm.ToLower();
                query = query.Where(a =>
                    a.Equipement.design.ToLower().Contains(lowerSearch) ||
                    a.Unite.designation.ToLower().Contains(lowerSearch));
            }

            sortBy ??= "dateaffec";
            query = sortBy.ToLower() switch
            {
                "equipement" => ascending ? query.OrderBy(a => a.Equipement.design) : query.OrderByDescending(a => a.Equipement.design),
                "unite" => ascending ? query.OrderBy(a => a.Unite.designation) : query.OrderByDescending(a => a.Unite.designation),
                _ => ascending ? query.OrderBy(a => a.dateaffec) : query.OrderByDescending(a => a.dateaffec)
            };

            return await query.Select(a => new AffectationDTO
            {
                idaffec = a.idaffec,
                ideqpt = a.ideqpt,
                idunite = a.idunite,
                dateaffec = a.dateaffec,
                designationEquipement = a.Equipement.design,
                designationUnite = a.Unite.designation
            }).ToListAsync();
        }

        public async Task<AffectationDTO> CreateAsync(CreateAffectationDTO dto)
        {
            // Vérifier si l'équipement existe
            var equipement = await _context.Equipements.FindAsync(dto.ideqpt);
            if (equipement == null)
                throw new Exception("Équipement non trouvé");

            // Vérifier si l'unité existe
            var unite = await _context.Unites.FindAsync(dto.idunite);
            if (unite == null)
                throw new Exception("Unité non trouvée");

            // Créer l'affectation
            var affectation = new Affectation
            {
                ideqpt = dto.ideqpt,
                idunite = dto.idunite,
                dateaffec = dto.dateaffec
            };

            _context.Affectations.Add(affectation);

            // Mettre à jour l'unité de l'équipement
            equipement.idunite = dto.idunite;
            _context.Equipements.Update(equipement);

            await _context.SaveChangesAsync();

            return new AffectationDTO
            {
                idaffec = affectation.idaffec,
                ideqpt = affectation.ideqpt,
                idunite = affectation.idunite,
                dateaffec = affectation.dateaffec,
                designationEquipement = equipement.design,
                designationUnite = unite.designation
            };
        }

        public async Task<AffectationDTO?> GetByIdAsync(int id)
        {
            var affectation = await _context.Affectations
                .Include(a => a.Equipement)
                .Include(a => a.Unite)
                .FirstOrDefaultAsync(a => a.idaffec == id);

            if (affectation == null) return null;

            return new AffectationDTO
            {
                idaffec = affectation.idaffec,
                ideqpt = affectation.ideqpt,
                idunite = affectation.idunite,
                dateaffec = affectation.dateaffec,
                designationEquipement = affectation.Equipement.design,
                designationUnite = affectation.Unite.designation
            };
        }
    }
} 