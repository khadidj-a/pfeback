using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;

namespace PFE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganeCaracteristiqueController : ControllerBase
    {
        private readonly IOrganeCaracteristiqueService _service;

        public OrganeCaracteristiqueController(IOrganeCaracteristiqueService service)
        {
            _service = service;
        }

        [HttpGet("organe/{id_organe}")]
        public async Task<ActionResult<IEnumerable<OrganeCaracteristiqueDTO>>> GetByOrganeId(int id_organe)
        {
            var caracteristiques = await _service.GetByOrganeIdAsync(id_organe);
            return Ok(caracteristiques);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<OrganeCaracteristiqueDTO>>> Create(CreateOrganeCaracteristiqueDTO dto)
        {
            var caracteristiques = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByOrganeId), new { id_organe = dto.id_organe }, caracteristiques);
        }

        [HttpPut("organe/{id_organe}/caracteristique/{id_caracteristique}")]
        public async Task<ActionResult<OrganeCaracteristiqueDTO>> Update(int id_organe, int id_caracteristique, UpdateOrganeCaracteristiqueDTO dto)
        {
            var caracteristique = await _service.UpdateAsync(id_organe, id_caracteristique, dto);
            if (caracteristique == null)
                return NotFound();
            return Ok(caracteristique);
        }
    }
} 