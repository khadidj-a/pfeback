using System.Collections.Generic;
using System.Threading.Tasks;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface ICategorieService
    {
        Task<IEnumerable<CategorieDTO>> GetAllCategoriesAsync(string? searchTerm = null, string? sortBy = null, bool ascending = true);
        Task<CategorieDTO?> GetCategorieByIdAsync(int id);
        Task<CategorieDTO> CreateCategorieAsync(CreateCategorieDTO categorieDto);
        Task<CategorieDTO?> UpdateCategorieAsync(int id, UpdateCategorieDTO categorieDto);
        Task<bool> DeleteCategorieAsync(int id);
        Task<bool> CanDeleteCategorieAsync(int id);
    }
}
