using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public class CategorieService : ICategorieService
    {
        private readonly ApplicationDbContext _context;

        public CategorieService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategorieDTO>> GetAllCategoriesAsync(string? searchTerm = null, string? sortBy = null, bool ascending = true)
        {
            var query = _context.Categories.AsQueryable();

            // Filtrage (Recherche)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var lowerSearch = searchTerm.ToLower();
                query = query.Where(c => 
                    (c.codecategorie != null && c.codecategorie.Contains(searchTerm)) ||
                    c.designation.Contains(searchTerm));
            }

            // Tri (Défaut : idcategorie)
            sortBy ??= "idcategorie";

            query = sortBy.ToLower() switch
            {
                "codecategorie" => ascending ? query.OrderBy(c => c.codecategorie) : query.OrderByDescending(c => c.codecategorie),
                "designation" => ascending ? query.OrderBy(c => c.designation) : query.OrderByDescending(c => c.designation),
                _ => ascending ? query.OrderBy(c => c.idcategorie) : query.OrderByDescending(c => c.idcategorie)
            };

            var categories = await query.ToListAsync();

            return categories.Select(c => new CategorieDTO
            {
                idcategorie = c.idcategorie,
                codecategorie = c.codecategorie,
                designation = c.designation
            });
        }

        public async Task<CategorieDTO?> GetCategorieByIdAsync(int id)
        {
            var categorie = await _context.Categories.FirstOrDefaultAsync(c => c.idcategorie == id);
            if (categorie == null) return null;

            return new CategorieDTO
            {
                idcategorie = categorie.idcategorie,
                codecategorie = categorie.codecategorie,
                designation = categorie.designation
            };
        }

        public async Task<CategorieDTO> CreateCategorieAsync(CreateCategorieDTO categorieDto)
        {
            var categorie = new Categorie
            {
                designation = categorieDto.designation
            };

            _context.Categories.Add(categorie);
            await _context.SaveChangesAsync();

            return new CategorieDTO
            {
                idcategorie = categorie.idcategorie,
                codecategorie = categorie.codecategorie,
                designation = categorie.designation
            };
        }

        public async Task<CategorieDTO?> UpdateCategorieAsync(int id, UpdateCategorieDTO categorieDto)
        {
            var categorie = await _context.Categories.FirstOrDefaultAsync(c => c.idcategorie == id);
            if (categorie == null) return null;

            categorie.designation = categorieDto.designation;
            await _context.SaveChangesAsync();

            return new CategorieDTO
            {
                idcategorie = categorie.idcategorie,
                codecategorie = categorie.codecategorie,
                designation = categorie.designation
            };
        }

        public async Task<bool> CanDeleteCategorieAsync(int id)
        {
            return !await _context.Equipements.AnyAsync(e => e.idCat == id);
        }

        public async Task<bool> DeleteCategorieAsync(int id)
        {
            if (!await CanDeleteCategorieAsync(id)) return false;

            var categorie = await _context.Categories.FirstOrDefaultAsync(c => c.idcategorie == id);
            if (categorie == null) return false;

            _context.Categories.Remove(categorie);
            await _context.SaveChangesAsync();
            return true;
        }
         public async Task<int> GetCategorieCountAsync()
        {
            return await _context.Categories.CountAsync();
        }
    }
}
