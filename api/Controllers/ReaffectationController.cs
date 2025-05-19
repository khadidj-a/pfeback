using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;


namespace PFE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReaffectationController : ControllerBase
    {
        private readonly IReaffectationService _service;
        private readonly ApplicationDbContext _context;

        public ReaffectationController(IReaffectationService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        // GET: api/Reaffectation
        [HttpGet]
        //[Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> GetAll([FromQuery] string? search = null, [FromQuery] string? sortBy = null, [FromQuery] string? order = "asc")
        {
            var list = await _service.GetAllAsync(search, sortBy, order);
            return Ok(list);
        }

        // POST: api/Reaffectation
        [HttpPost]
        //[Authorize(Roles = "Admin Métier")]
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

        // GET: api/Reaffectation/{id}
        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

  // GET: api/Reaffectation/byunite?idunite=1&search=...&sortBy=...&order=asc
[HttpGet("byunite")]
public async Task<ActionResult<IEnumerable<ReaffectationDTO>>> GetReaffectationsByUnite(
    [FromQuery] int idunite,
    [FromQuery] string? search = null,
    [FromQuery] string? sortBy = null,
    [FromQuery] string order = "asc")
{
    var reaffectations = await _service.GetByUniteAsync(idunite, search, sortBy, order);
    return Ok(reaffectations);
}


        // GET: api/Reaffectation/count
        [HttpGet("count")]
        //[Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<int>> GetReaffectationCount()
        {
            return await _service.GetReaffectationCountAsync();
        }

        // GET: api/Reaffectation/equipement/{id}/etat
        [HttpGet("equipement/{id}/etat")]
        public async Task<IActionResult> GetEtatEquipement(int id)
        {
            var etat = await _service.GetEtatEquipementAsync(id);
            return etat == null ? NotFound("Équipement introuvable") : Ok(etat);
        }

        // GET: api/Reaffectation/equipements/{id}/unite
        [HttpGet("equipements/{id}/unite")]
        public IActionResult GetUniteByEquipementId(int id)
        {
           var equipement = _context.Equipements.Include(e => e.Unite).FirstOrDefault(e => e.idEqpt == id);
if (equipement == null || equipement.idunite == null)
    return NotFound();

var unite = _context.Unites
    .Where(u => u.idunite == equipement.idunite)
    .Select(u => new { idunite = u.idunite, designation = u.designation })
    .FirstOrDefault();


            return Ok(unite);
        }
        [HttpGet("count/unite/{idUnite}")]
public async Task<int> GetreaffCountByUnite(int idUnite)
{
    
    return await _context.Reaffectations
       
        .Where(r => r.iduniteemt == idUnite )
        .CountAsync();
}

    }
}
