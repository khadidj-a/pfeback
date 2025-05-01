using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;
using PFE_PROJECT.Helpers; // pour RoleHelper
using Microsoft.AspNetCore.Authorization;

namespace PFE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypeEquipController : ControllerBase
    {
        private readonly ITypeEquipService _typeEquipService;

        public TypeEquipController(ITypeEquipService typeEquipService)
        {
            _typeEquipService = typeEquipService;
        }

        // GET: api/TypeEquip
        [HttpGet]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<IEnumerable<TypeEquipDTO>>> GetTypeEquips(
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool ascending = true)
        {
            var types = await _typeEquipService.GetAllTypeEquipsAsync(searchTerm, sortBy, ascending);
            return Ok(types);
        }

        // GET: api/TypeEquip/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<TypeEquipDTO>> GetTypeEquip(int id)
        {
            var typeEquip = await _typeEquipService.GetTypeEquipByIdAsync(id);
            if (typeEquip == null)
                return NotFound();

            return Ok(typeEquip);
        }

        // POST: api/TypeEquip
        [HttpPost]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<TypeEquipDTO>> CreateTypeEquip(CreateTypeEquipDTO typeEquipDto)
        {
            var createdTypeEquip = await _typeEquipService.CreateTypeEquipAsync(typeEquipDto);
            return CreatedAtAction(nameof(GetTypeEquip), new { id = createdTypeEquip.idtypequip }, createdTypeEquip);
        }

        // PUT: api/TypeEquip/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> UpdateTypeEquip(int id, UpdateTypeEquipDTO typeEquipDto)
        {
            var updatedTypeEquip = await _typeEquipService.UpdateTypeEquipAsync(id, typeEquipDto);
            if (updatedTypeEquip == null)
                return NotFound();

            return Ok(updatedTypeEquip);
        }

        // DELETE: api/TypeEquip/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> DeleteTypeEquip(int id)
        {
            var result = await _typeEquipService.DeleteTypeEquipAsync(id);
            if (!result)
                return BadRequest("Ce type d’équipement est utilisé et ne peut pas être supprimé.");

            return NoContent();
        }
    }
}
