using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using PFE_PROJECT.Helpers; // Pour accéder aux constantes de rôles

namespace PFE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaracteristiqueController : ControllerBase
    {
        private readonly ICaracteristiqueService _service;

        public CaracteristiqueController(ICaracteristiqueService service)
        {
            _service = service;
        }

        // Les deux rôles peuvent consulter la liste
        [HttpGet]
// [Authorize(Roles = "Admin Métier,Admin IT")]
public async Task<ActionResult<IEnumerable<CaracteristiqueDTO>>> GetAll(
    [FromQuery] string? searchTerm = null,
    [FromQuery] string? sortBy = null,
    [FromQuery] bool ascending = true)
{
    var result = await _service.GetAllCaracteristiquesAsync(searchTerm, sortBy, ascending);
    return Ok(result);
}


        // Les deux rôles peuvent consulter un élément précis
        [HttpGet("{id}")]
       // [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<CaracteristiqueDTO>> GetById(int id)
        {
            var result = await _service.GetCaracteristiqueByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // Seul l'Administrateur Métier peut créer
        [HttpPost]
     //   [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<CaracteristiqueDTO>> Create(CreateCaracteristiqueDTO dto)
        {
            var created = await _service.CreateCaracteristiqueAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.id_caracteristique }, created);
        }

        // Seul l'Administrateur Métier peut modifier
        [HttpPut("{id}")]
       // [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> Update(int id, UpdateCaracteristiqueDTO dto)
        {
            var updated = await _service.UpdateCaracteristiqueAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        // Seul l'Administrateur Métier peut supprimer
        [HttpDelete("{id}")]
       // [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _service.CanDeleteCaracteristiqueAsync(id))
                return BadRequest("Cette caractéristique est utilisée par un équipement.");

            var result = await _service.DeleteCaracteristiqueAsync(id);
            return result ? NoContent() : NotFound();
        }
        [HttpGet("canDelete/{id}")]
public async Task<ActionResult<bool>> CanDelete(int id)
{
    var canDelete = await _service.CanDeleteCaracteristiqueAsync(id);
    return Ok(canDelete);
}

        [HttpGet("type/{typeId}/marque/{marqueId}")]
        public async Task<ActionResult<IEnumerable<CaracteristiqueDTO>>> GetByTypeAndMarque(int typeId, int marqueId)
        {
            var caracteristiques = await _service.GetByTypeAndMarqueAsync(typeId, marqueId);
            return Ok(caracteristiques);
        }
    }
}
