using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;
using PFE_PROJECT.Helpers; // pour RoleHelper
using Microsoft.AspNetCore.Authorization;

namespace PFE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PretController : ControllerBase
    {
        private readonly IPretService _service;

        public PretController(IPretService service)
        {
            _service = service;
        }

        // GET: api/Pret
        [HttpGet]
[Authorize(Roles = "Admin Métier,Admin IT,Responsable Unité")]
public async Task<IActionResult> GetAll([FromQuery] string? search = null, [FromQuery] string? sortBy = null, [FromQuery] string? order = "asc")
{
    // Si c'est un responsable d’unité, on filtre
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


        // POST: api/Pret
        [HttpPost]
[Authorize(Roles = "Admin Métier,Admin IT")]
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
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> GetById(int id)
        {
            var pret = await _service.GetByIdAsync(id);
            return pret == null ? NotFound() : Ok(pret);
        }
    }
}
