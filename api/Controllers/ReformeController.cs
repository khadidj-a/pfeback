using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;
using PFE_PROJECT.Helpers; // pour RoleHelper
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
namespace PFE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReformeController : ControllerBase
    {
        private readonly IReformeService _service;
        private readonly ApplicationDbContext _context;

       public ReformeController(IReformeService service, ApplicationDbContext context)
{
    _service = service;
    _context = context;
}


        // GET: api/Reforme
        [HttpGet]
       // [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> GetAll([FromQuery] string? search = null, [FromQuery] string? sortBy = null, [FromQuery] string? order = "asc")
        {
            var result = await _service.GetAllAsync(search, sortBy, order);
            return Ok(result);
        }

        // GET: api/Reforme/5
        [HttpGet("{id}")]
    //   [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> GetById(int id)
        {
            var r = await _service.GetByIdAsync(id);
            return r == null ? NotFound() : Ok(r);
        }
// GET: api/Reforme/byunite?idunite=1&search=...&sortBy=...&order=asc
[HttpGet("byunite")]
public async Task<ActionResult<IEnumerable<ReformeDTO>>> GetReformesByUnite(
    [FromQuery] int idunite,
    [FromQuery] string? search = null,
    [FromQuery] string? sortBy = null,
    [FromQuery] string order = "asc")
{
    var reformes = await _service.GetByUniteAsync(idunite, search, sortBy, order);
    return Ok(reformes);
}



       [HttpPost]
public async Task<IActionResult> Create([FromBody] CreateReformeDTO dto)
{
    try
    {
        var result = await _service.CreateAsync(dto);
        return Ok(result);
    }
    catch (Exception ex)
{
    return BadRequest(new { message = ex.Message, detail = ex.InnerException?.Message });
}

}

        [HttpGet("count")]
       // [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<int>> GetReformeCount()
        {
            return await _service.GetReformeCountAsync();
        }

     [HttpGet("equipement/{id}/etat")]
public async Task<IActionResult> GetEtatEquipement(int id)
{
    var etat = await _service.GetEtatEquipementAsync(id);

    if (etat == null)
        return NotFound("Équipement introuvable");

    return Ok(etat); // <--- retourne bien une string comme "En Service"
}


[HttpGet("exists/{numeroDecision}")]
public async Task<ActionResult<bool>> CheckNumeroDecisionExists(string numeroDecision)
{
    var exists = await _service.NumeroDecisionExistsAsync(numeroDecision);
    return Ok(exists);
}

[HttpGet("count/unite/{idUnite}")]
public async Task<int> GetReformeCountByUnite(int idUnite)
{
    
    return await _context.Reformes
       
        .Where(r => r.Equipement.idunite == idUnite)
        .CountAsync();
}


    }
    
}
