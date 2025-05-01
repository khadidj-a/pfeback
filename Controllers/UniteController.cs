using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using PFE_PROJECT.Helpers; // pour RoleHelper

using Microsoft.AspNetCore.Authorization;
namespace PFE_PROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniteController : ControllerBase
    {
        private readonly IUniteService _uniteService;

        public UniteController(IUniteService uniteService)
        {
            _uniteService = uniteService;
        }

        // GET: api/Unite
        [HttpGet]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<IEnumerable<UniteDTO>>> GetUnites(
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool ascending = true)
        {
            var result = await _uniteService.GetAllUnitesAsync(searchTerm, sortBy, ascending);
            return Ok(result);
        }

        // GET: api/Unite/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<UniteDTO>> GetUnite(int id)
        {
            var unite = await _uniteService.GetUniteByIdAsync(id);
            if (unite == null) return NotFound();
            return Ok(unite);
        }

        // POST: api/Unite
        [HttpPost]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<UniteDTO>> CreateUnite(CreateUniteDTO dto)
        {
            var created = await _uniteService.CreateUniteAsync(dto);
            return CreatedAtAction(nameof(GetUnite), new { id = created.idunite }, created);
        }

        // PUT: api/Unite/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> UpdateUnite(int id, UpdateUniteDTO dto)
        {
            var updated = await _uniteService.UpdateUniteAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE: api/Unite/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> DeleteUnite(int id)
        {
            if (!await _uniteService.CanDeleteUniteAsync(id))
                return BadRequest("Cette unité contient des équipements et ne peut pas être supprimée.");

            var result = await _uniteService.DeleteUniteAsync(id);
            return result ? NoContent() : NotFound();
        }

        // GET: api/Unite/wilayas
        [HttpGet("wilayas")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<IEnumerable<Wilaya>>> GetWilayas()
        {
            return Ok(await _uniteService.GetAllWilayasAsync());
        }

        // GET: api/Unite/regions
        [HttpGet("regions")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<IEnumerable<Region>>> GetRegions()
        {
            return Ok(await _uniteService.GetAllRegionsAsync());
        }
    }
}

