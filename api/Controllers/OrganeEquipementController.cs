using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;

namespace PFE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganeEquipementController : ControllerBase
    {
        private readonly IOrganeEquipementService _service;

        public OrganeEquipementController(IOrganeEquipementService service)
        {
            _service = service;
        }

        [HttpGet("equipement/{ideqpt}")]
        public async Task<ActionResult<IEnumerable<OrganeEquipementDTO>>> GetByEquipementId(int ideqpt)
        {
            var organes = await _service.GetByEquipementIdAsync(ideqpt);
            return Ok(organes);
        }

        [HttpGet("type/{typeId}/marque/{marqueId}")]
        public async Task<ActionResult<IEnumerable<OrganeEquipementDTO>>> GetByTypeAndMarque(int typeId, int marqueId)
        {
            var organes = await _service.GetByTypeAndMarqueAsync(typeId, marqueId);
            return Ok(organes);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<OrganeEquipementDTO>>> Create(CreateOrganeEquipementDTO dto)
        {
            var organes = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByEquipementId), new { ideqpt = dto.ideqpt }, organes);
        }

        [HttpPut("equipement/{ideqpt}/organe/{idorg}")]
        public async Task<ActionResult<OrganeEquipementDTO>> Update(int ideqpt, int idorg, UpdateOrganeEquipementDTO dto)
        {
            var organe = await _service.UpdateAsync(ideqpt, idorg, dto);
            if (organe == null)
                return NotFound();
            return Ok(organe);
        }
    }
} 