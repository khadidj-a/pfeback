using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;

namespace PFE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupeCaracteristiqueController : ControllerBase
    {
        private readonly IGroupeCaracteristiqueService _service;

        public GroupeCaracteristiqueController(IGroupeCaracteristiqueService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupeCaracteristiqueDTO>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("by-groupe/{idgrpidq}")]
        public async Task<ActionResult<IEnumerable<GroupeCaracteristiqueDTO>>> GetByGroupeId(int idgrpidq)
        {
            var result = await _service.GetByGroupeIdAsync(idgrpidq);
            return Ok(result);
        }

        [HttpDelete("by-groupe/{idgrpidq}")]
        public async Task<ActionResult> DeleteByGroupeId(int idgrpidq)
        {
            var result = await _service.DeleteByGroupeIdAsync(idgrpidq);
            if (!result) return NotFound();
            return NoContent();
        }
    }
} 