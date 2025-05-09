using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;
using PFE_PROJECT.Helpers; // pour RoleHelper
using Microsoft.AspNetCore.Authorization;

namespace PFE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReaffectationController : ControllerBase
    {
        private readonly IReaffectationService _service;

        public ReaffectationController(IReaffectationService service)
        {
            _service = service;
        }

        // GET: api/Reaffectation
        [HttpGet]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> GetAll([FromQuery] string? search = null, [FromQuery] string? sortBy = null, [FromQuery] string? order = "asc")
        {
            var list = await _service.GetAllAsync(search, sortBy, order);
            return Ok(list);
        }

        // POST: api/Reaffectation
        [HttpPost]
        [Authorize(Roles = "Admin Métier")]
        public async Task<IActionResult> Create(CreateReaffectationDTO dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.idreaf }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        // GET: api/Reaffectation/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        // GET: api/Reaffectation/unite
[HttpGet("unite")]
[Authorize(Roles = "Responsable Unité")]
public async Task<IActionResult> GetByUnite([FromQuery] string? search = null, [FromQuery] string? sortBy = null, [FromQuery] string? order = "asc")
{
    // 🔐 Récupérer l'id de l'unité depuis les claims utilisateur
    var idUniteClaim = User.Claims.FirstOrDefault(c => c.Type == "idunite")?.Value;

    if (string.IsNullOrEmpty(idUniteClaim) || !int.TryParse(idUniteClaim, out int idUnite))
        return Unauthorized("Unité non reconnue pour cet utilisateur.");

    var result = await _service.GetByUniteAsync(idUnite, search, sortBy, order);
    return Ok(result);
}

    }
}
