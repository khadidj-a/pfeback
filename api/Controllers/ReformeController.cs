using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;
using PFE_PROJECT.Helpers; // pour RoleHelper
using Microsoft.AspNetCore.Authorization;

namespace PFE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReformeController : ControllerBase
    {
        private readonly IReformeService _service;

        public ReformeController(IReformeService service)
        {
            _service = service;
        }

        // GET: api/Reforme
        [HttpGet]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> GetAll([FromQuery] string? search = null, [FromQuery] string? sortBy = null, [FromQuery] string? order = "asc")
        {
            var result = await _service.GetAllAsync(search, sortBy, order);
            return Ok(result);
        }

        // GET: api/Reforme/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> GetById(int id)
        {
            var r = await _service.GetByIdAsync(id);
            return r == null ? NotFound() : Ok(r);
        }
[HttpGet("unite")]
[Authorize(Roles = "Responsable Unité")]
public async Task<IActionResult> GetByUniteAsync([FromQuery] string? search = null, [FromQuery] string? sortBy = null, [FromQuery] string? order = "asc")
{
    var idUniteClaim = User.Claims.FirstOrDefault(c => c.Type == "idunite");
    if (idUniteClaim == null)
        return Forbid("Aucune unité associée à cet utilisateur.");

    if (!int.TryParse(idUniteClaim.Value, out int idUniteParsed))
        return BadRequest("Identifiant d'unité invalide.");

    var result = await _service.GetByUniteAsync(idUniteParsed, search, sortBy, order);

    // Convertir les entités Reforme en ReformeDTO avant de les retourner
    var resultDTO = result.Select(r => new ReformeDTO
    {
        idref = r.idref,
        ideqpt = r.ideqpt,
        motifref = r.motifref,
        dateref = r.dateref,
        numdes = r.numdes
    }).ToList();

    return Ok(resultDTO);
}



        // POST: api/Reforme
        [HttpPost]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> Create(CreateReformeDTO dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.idref }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
