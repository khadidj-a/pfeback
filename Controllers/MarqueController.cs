using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;
using PFE_PROJECT.Helpers; // pour RoleHelper

namespace PFE_PROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarqueController : ControllerBase
    {
        private readonly IMarqueService _marqueService;

        public MarqueController(IMarqueService marqueService)
        {
            _marqueService = marqueService;
        }

        // GET: api/Marque
        [HttpGet]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<IEnumerable<MarqueDTO>>> GetMarques(
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool ascending = true)
        {
            var marques = await _marqueService.GetAllMarquesAsync(searchTerm, sortBy, ascending);
            return Ok(marques);
        }

        // GET: api/Marque/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<MarqueDTO>> GetMarque(int id)
        {
            var marque = await _marqueService.GetMarqueByIdAsync(id);
            if (marque == null)
                return NotFound();

            return Ok(marque);
        }

        // POST: api/Marque
        [HttpPost]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<MarqueDTO>> CreateMarque(CreateMarqueDTO marqueDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdMarque = await _marqueService.CreateMarqueAsync(marqueDto);
                return CreatedAtAction(nameof(GetMarque), new { id = createdMarque.idmarque }, createdMarque);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur est survenue lors de la création de la marque: {ex.Message}");
            }
        }

        // PUT: api/Marque/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> UpdateMarque(int id, UpdateMarqueDTO marqueDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedMarque = await _marqueService.UpdateMarqueAsync(id, marqueDto);
                if (updatedMarque == null)
                    return NotFound();

                return Ok(updatedMarque);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur est survenue lors de la mise à jour de la marque: {ex.Message}");
            }
        }

        // DELETE: api/Marque/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> DeleteMarque(int id)
        {
            var canDelete = await _marqueService.CanDeleteMarqueAsync(id);
            if (!canDelete)
                return BadRequest("Cette marque ne peut pas être supprimée car elle est utilisée par des équipements.");

            var result = await _marqueService.DeleteMarqueAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // GET: api/Marque/CanDelete/5
        [HttpGet("CanDelete/{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<bool>> CanDeleteMarque(int id)
        {
            var canDelete = await _marqueService.CanDeleteMarqueAsync(id);
            return Ok(canDelete);
        }
    }
}
