using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;
using System.Threading.Tasks;

namespace PFE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipementController : ControllerBase
    {
        private readonly IEquipementService _equipementService;

        public EquipementController(IEquipementService equipementService)
        {
            _equipementService = equipementService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipementDTO>>> GetAll([FromQuery] EquipementFilterDTO? filter)
        {
            var equipements = await _equipementService.GetAllAsync(filter);
            return Ok(equipements);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EquipementDTO>> GetById(int id)
        {
            var equipement = await _equipementService.GetByIdAsync(id);
            if (equipement == null) return NotFound();
            return Ok(equipement);
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<EquipementDTO>> GetByCode(string code)
        {
            var equipement = await _equipementService.GetByCodeAsync(code);
            if (equipement == null) return NotFound();
            return Ok(equipement);
        }

        [HttpPost]
        public async Task<ActionResult<EquipementDTO>> Create(CreateEquipementDTO dto)
        {
            try
            {
                var equipement = await _equipementService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = equipement.idEqpt }, equipement);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EquipementDTO>> Update(int id, UpdateEquipementDTO dto)
        {
            try
            {
                var equipement = await _equipementService.UpdateAsync(id, dto);
                if (equipement == null) return NotFound();
                return Ok(equipement);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _equipementService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
} 