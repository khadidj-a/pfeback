using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;
using PFE_PROJECT.Helpers; // pour RoleHelper

namespace PFE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupeIdentiqueController : ControllerBase
    {
        private readonly IGroupeIdentiqueService _service;

        public GroupeIdentiqueController(IGroupeIdentiqueService service)
        {
            _service = service;
        }

        // Consultation (tous)
        [HttpGet]
        //[Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<IEnumerable<GroupeIdentiqueDTO>>> GetAll(
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool ascending = true)
        {
            var result = await _service.GetAllAsync(searchTerm, sortBy, ascending);
            return Ok(result);
        }

        // Consultation par ID (tous)
        [HttpGet("{id}")]
       // [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<GroupeDetailsDTO>> GetGroupDetails(int id)
        {
            var result = await _service.GetGroupDetailsAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        // Mise à jour : seulement Admin Métier
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> Update(int id, UpdateGroupeIdentiqueDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        // Suppression : seulement Admin Métier
       // Suppression : seulement Admin Métier
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin Métier,Admin IT")] 
        public async Task<IActionResult> Delete(int id)
        {
            var canDelete = await _service.CanDeleteGroupeAsync(id);
            if (!canDelete)
                return BadRequest("Ce groupe ne peut pas être supprimée car elle est utilisée par des équipements.");
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("count")]
       // [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<int>> GetGroupeCount()
        {
             return await _service.GetGroupeCountAsync();
        }
        
        [HttpGet("CanDelete/{id}")]
       // [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<bool>> CanDelete(int id)
        {
            var canDelete = await _service.CanDeleteGroupeAsync(id);
            return Ok(canDelete);
        }
          [HttpGet("byTypeAndMarque")]
        public async Task<ActionResult<IEnumerable<GroupeIdentiqueDTO>>> GetByTypeAndMarque(
            [FromQuery] int typeId,
            [FromQuery] int marqueId)
        {
            var result = await _service.GetByTypeAndMarqueAsync(typeId, marqueId);
            return Ok(result);
        }

        
    }
}
