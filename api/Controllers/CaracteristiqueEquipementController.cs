using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;

namespace PFE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaracteristiqueEquipementController : ControllerBase
    {
        private readonly ICaracteristiqueEquipementService _service;

        public CaracteristiqueEquipementController(ICaracteristiqueEquipementService service)
        {
            _service = service;
        }

        [HttpGet("equipement/{ideqpt}")]
        public async Task<ActionResult<IEnumerable<CaracteristiqueEquipementDTO>>> GetByEquipementId(int ideqpt, [FromQuery] bool showValue = true)
        {
            var caracteristiques = await _service.GetByEquipementIdAsync(ideqpt, showValue);
            return Ok(caracteristiques);
        }

        [HttpGet("type/{typeId}/marque/{marqueId}")]
        public async Task<ActionResult<IEnumerable<CaracteristiqueEquipementDTO>>> GetByTypeAndMarque(int typeId, int marqueId)
        {
            var caracteristiques = await _service.GetByTypeAndMarqueAsync(typeId, marqueId);
            return Ok(caracteristiques);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<CaracteristiqueEquipementDTO>>> Create(BulkCreateCaracteristiqueEquipementDTO dto)
        {
            var caracteristiques = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByEquipementId), new { ideqpt = dto.ideqpt }, caracteristiques);
        }

        [HttpPost("bulk")]
        public async Task<ActionResult<IEnumerable<CaracteristiqueEquipementDTO>>> BulkCreate(BulkCreateCaracteristiqueEquipementDTO dto)
        {
            var caracteristiques = await _service.BulkCreateAsync(dto);
            return CreatedAtAction(nameof(GetByEquipementId), new { ideqpt = dto.ideqpt }, caracteristiques);
        }

        [HttpPut("equipement/{ideqpt}/caracteristique/{idcarac}")]
        public async Task<ActionResult<CaracteristiqueEquipementDTO>> Update(int ideqpt, int idcarac, UpdateCaracteristiqueEquipementDTO dto)
        {
            var caracteristique = await _service.UpdateAsync(ideqpt, idcarac, dto);
            if (caracteristique == null)
                return NotFound();
            return Ok(caracteristique);
        }

        [HttpPost("add")]
        public async Task<ActionResult<CaracteristiqueEquipementDTO>> AddCaracteristique(AddCaracteristiqueEquipementDTO dto)
        {
            var result = await _service.AddCaracteristiqueAsync(dto);
            if (result == null)
                return BadRequest("Failed to add caracteristique");
            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteCaracteristique(DeleteCaracteristiqueEquipementDTO dto)
        {
            var success = await _service.DeleteCaracteristiqueAsync(dto);
            if (!success)
                return NotFound();
            return Ok();
        }

        [HttpPut("modify")]
        public async Task<ActionResult<CaracteristiqueEquipementDTO>> ModifyCaracteristique(ModifyCaracteristiqueEquipementDTO dto)
        {
            var result = await _service.ModifyCaracteristiqueAsync(dto);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
} 