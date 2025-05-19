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
    public class PretController : ControllerBase
    {
        private readonly IPretService _service;

        private readonly ApplicationDbContext _context;
   public PretController(IPretService service, ApplicationDbContext context)
{
    _service = service;
    _context = context;
}


        // GET: api/Pret
// GET: api/Pret
[HttpGet]
public async Task<IActionResult> GetAll([FromQuery] string? search = null, [FromQuery] string? sortBy = null, [FromQuery] string? order = "asc")
{
    // Mise à jour avant la récupération
    await _service.MettreAJourPositionPhysiqueAsync();

    int? idUnite = null;
    if (User.IsInRole("Responsable Unité"))
    {
        var idClaim = User.FindFirst("idunite");
        if (idClaim != null)
        {
            idUnite = int.Parse(idClaim.Value);
        }
    }

    var list = await _service.GetAllAsync(search, sortBy, order, idUnite);
    return Ok(list);
}

[HttpGet("byunite")]
public async Task<ActionResult<IEnumerable<PretDTO>>> GetPretsByUnite([FromQuery] int idunite, string? search = null, string? sortBy = null, string order = "asc")
{
    // Mise à jour avant la récupération
await _service.MettreAJourPositionPhysiqueAsync();

    var prets = await _service.GetByUniteAsync(idunite, search, sortBy, order);
    return Ok(prets);
}


        // POST: api/Pret
        [HttpPost]
//[Authorize(Roles = "Admin Métier,Admin IT")]
public async Task<IActionResult> Create(CreatePretDTO dto)
{
    try
    {
        // On récupère le rôle courant
        var role = User.IsInRole("Responsable Unité") ? "Responsable Unité" : "Admin Métier";

        var created = await _service.CreateAsync(dto, role);
        return CreatedAtAction(nameof(GetById), new { id = created.idpret }, created);
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}


        // GET: api/Pret/5
        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> GetById(int id)
        {
            var pret = await _service.GetByIdAsync(id);
            return pret == null ? NotFound() : Ok(pret);
        }
         [HttpGet("count")]
       // [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<int>> GetPretCount()
        {
             return await _service.GetPretCountAsync();
        }

        
     [HttpGet("equipement/{id}/etat")]
public async Task<IActionResult> GetEtatEquipement(int id)
{
    var etat = await _service.GetEtatEquipementAsync(id);

    if (etat == null)
        return NotFound("Équipement introuvable");

    return Ok(etat); // <--- retourne bien une string comme "En Service"
}



// Exemple côté .NET

        [HttpGet("count/unite/{idUnite}")]
public async Task<int> GetpretCountByUnite(int idUnite)
{
    
    return await _context.Prets
       
        .Where(p => p.iduniteemt == idUnite || p.idunite == idUnite)
        .CountAsync();
}
[HttpPost("mettreajourposition")]
public async Task<IActionResult> MettreAJourPositionPhysique()
{
    try
    {
        await _service.MettreAJourPositionPhysiqueAsync();
        return Ok(new { message = "Mise à jour de la position physique effectuée avec succès." });
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}


    }

}
