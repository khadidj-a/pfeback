using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace PFE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class AffectationController : ControllerBase
    {
        private readonly IAffectationService _service;

        public AffectationController(IAffectationService service)
        {
            _service = service;
        }

        [HttpGet]
       // [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<IEnumerable<AffectationDTO>>> GetAll(
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool ascending = true)
        {
            try
            {
                var result = await _service.GetAllAsync(searchTerm, sortBy, ascending);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Une erreur est survenue lors de la récupération des affectations", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<AffectationDTO>> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return result == null ? NotFound(new { message = $"Affectation avec l'ID {id} non trouvée" }) : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Une erreur est survenue lors de la récupération de l'affectation", error = ex.Message });
            }
        }

        [HttpPost]
        //[Authorize(Roles = "Admin Métier")]
        public async Task<ActionResult<AffectationDTO>> Create([FromBody] CreateAffectationDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Données invalides", errors = ModelState.Values.SelectMany(v => v.Errors) });
                }

                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.idaffec }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erreur lors de la création de l'affectation", error = ex.Message });
            }
        }
    }
} 