using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.DTOs;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;
// using Microsoft.AspNetCore.Authorization;

namespace PFE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganeController : ControllerBase
    {
        private readonly IOrganeService _service;

        public OrganeController(IOrganeService service)
        {
            _service = service;
        }

        // 🔵 GET: api/Organe
        [HttpGet]
        // [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<IEnumerable<OrganeDTO>>> GetAll(
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool ascending = true)
        {
            var organes = await _service.GetAllAsync(searchTerm, sortBy, ascending);
            return Ok(organes);
        }

        // 🔵 GET: api/Organe/5
        [HttpGet("{id}")]
        // [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<OrganeDTO>> GetById(int id)
        {
            var organe = await _service.GetByIdAsync(id);
            if (organe == null)
                return NotFound();

            return Ok(organe);
        }

        // 🟢 POST: api/Organe
        [HttpPost]
        // [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<OrganeDTO>> Create(CreateOrganeDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.id_organe }, created);
        }

        // 🟡 PUT: api/Organe/5
        [HttpPut("{id}")]
        // [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> Update(int id, UpdateOrganeDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        // 🔴 DELETE: api/Organe/5
        [HttpDelete("{id}")]
        // [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> Delete(int id)
        {
            var canDelete = await _service.CanDeleteAsync(id);
            if (!canDelete)
                return BadRequest("Cet organe est utilisé par un équipement.");

            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        // 🔵 GET: api/Organe/canDelete/5
        [HttpGet("canDelete/{id}")]
        public async Task<ActionResult<bool>> CanDelete(int id)
        {
            var result = await _service.CanDeleteAsync(id);
            return Ok(result);
        }
    }
}


