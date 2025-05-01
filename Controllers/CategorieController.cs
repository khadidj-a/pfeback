using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;
using PFE_PROJECT.Helpers;

namespace PFE_PROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorieController : ControllerBase
    {
        private readonly ICategorieService _categorieService;

        public CategorieController(ICategorieService categorieService)
        {
            _categorieService = categorieService;
        }

        // Lecture : autorisée pour Admin IT et Admin Métier
        [HttpGet]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<IEnumerable<CategorieDTO>>> GetCategories(
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool ascending = true)
        {
            var categories = await _categorieService.GetAllCategoriesAsync(searchTerm, sortBy, ascending);
            return Ok(categories);
        }

        // Lecture d’un élément précis
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<CategorieDTO>> GetCategorie(int id)
        {
            var categorie = await _categorieService.GetCategorieByIdAsync(id);
            if (categorie == null) return NotFound();
            return Ok(categorie);
        }

        // Création : seulement Admin Métier
        [HttpPost]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<CategorieDTO>> CreateCategorie(CreateCategorieDTO categorieDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdCategorie = await _categorieService.CreateCategorieAsync(categorieDto);
            return CreatedAtAction(nameof(GetCategorie), new { id = createdCategorie.idcategorie }, createdCategorie);
        }

        // Modification : seulement Admin Métier
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> UpdateCategorie(int id, UpdateCategorieDTO categorieDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedCategorie = await _categorieService.UpdateCategorieAsync(id, categorieDto);
            if (updatedCategorie == null) return NotFound();

            return Ok(updatedCategorie);
        }

        // Suppression : seulement Admin Métier
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> DeleteCategorie(int id)
        {
            if (!await _categorieService.CanDeleteCategorieAsync(id))
                return BadRequest("Cette catégorie est utilisée par un équipement.");

            var result = await _categorieService.DeleteCategorieAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
