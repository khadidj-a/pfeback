using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.DTOs;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;
using System.Threading.Tasks;

namespace PFE_PROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateurController : ControllerBase
    {
        private readonly IUtilisateurService _utilisateurService;

        public UtilisateurController(IUtilisateurService utilisateurService)
        {
            _utilisateurService = utilisateurService;
        }

        // Only Admin IT should be able to create users
        [HttpPost]
        [Authorize(Roles = "Admin IT")]  // Only Admin IT can perform this action
        public async Task<IActionResult> Create([FromBody] CreateUtilisateurDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid user data.");
            }

            try
            {
                // Map the DTO to the Utilisateur model
                var utilisateur = new Utilisateur
                {
                    idunite = dto.IdUnite,
                    nom = dto.Nom,
                    prenom = dto.Prenom,
                    numtel = dto.NumTel,
                    email = dto.Email,
                    motpasse = dto.MotPasse,
                    idrole = dto.IdRole,
                    Actif = "1" // Default value for "actif"
                };

                var createdUser = await _utilisateurService.CreateAsync(utilisateur);
                return CreatedAtAction(nameof(GetById), new { id = createdUser.iduser }, createdUser);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Only Admin IT should be able to update users
[HttpPut("{id}")]
[Authorize(Roles = "Admin IT")]
public async Task<IActionResult> Update(int id, [FromBody] UpdateUtilisateurDto dto)
{
    if (dto == null)
    {
        return BadRequest("Invalid user data.");
    }

    try
    {
        // D'abord, récupérer l'utilisateur existant
        var existingUser = await _utilisateurService.GetByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound($"Utilisateur avec ID {id} non trouvé.");
        }

        // Mettre à jour uniquement les champs fournis
        if (dto.IdUnite.HasValue)
            existingUser.idunite = dto.IdUnite.Value;
        
        if (!string.IsNullOrEmpty(dto.Nom))
            existingUser.nom = dto.Nom;
            
        if (!string.IsNullOrEmpty(dto.Prenom))
            existingUser.prenom = dto.Prenom;
            
        if (!string.IsNullOrEmpty(dto.NumTel))
            existingUser.numtel = dto.NumTel;
            
        if (!string.IsNullOrEmpty(dto.Email))
            existingUser.email = dto.Email;
            
        if (!string.IsNullOrEmpty(dto.MotPasse))
            existingUser.motpasse = dto.MotPasse;
            
        if (dto.IdRole.HasValue)
            existingUser.idrole = dto.IdRole.Value;

        // Appeler le service pour mettre à jour l'utilisateur
        var updatedUser = await _utilisateurService.UpdateAsync(existingUser);
        return Ok(updatedUser);
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(ex.Message);
    }
}
        // Only Admin IT should be able to delete users
        // [HttpDelete("{id}")]
        // [Authorize(Roles = "Admin IT")]
        // public async Task<IActionResult> Delete(int id)
        // {
        //     try
        //     {
        //         await _utilisateurService.DeleteAsync(id);
        //         return NoContent();
        //     }
        //     catch (InvalidOperationException ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        // }

        // Public endpoint to get user details (can be open or restricted)
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin IT, User")]  // Either Admin IT or a regular User can view their own data
        public async Task<IActionResult> GetById(int id)
        {
            var utilisateur = await _utilisateurService.GetByIdAsync(id);

            if (utilisateur == null)
            {
                return NotFound();
            }

            return Ok(utilisateur);
        }

        // Only Admin IT can view all users
        [HttpGet]
        [Authorize(Roles = "Admin IT")]
        public async Task<IActionResult> GetAll()
        {
            var utilisateurs = await _utilisateurService.GetAllAsync();
            return Ok(utilisateurs);
        }

        // Only Admin IT can activate/deactivate users
        [HttpPatch("{id}/activate")]
        [Authorize(Roles = "Admin IT")]
        public async Task<IActionResult> Activate(int id)
        {
            try
            {
                var utilisateur = await _utilisateurService.ActivateDeactivateAsync(id, true);
                if (utilisateur == null)
                {
                    return NotFound();
                }
                return Ok(utilisateur);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Only Admin IT can activate/deactivate users
        [HttpPatch("{id}/deactivate")]
        [Authorize(Roles = "Admin IT")]
        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                var utilisateur = await _utilisateurService.ActivateDeactivateAsync(id, false);
                if (utilisateur == null)
                {
                    return NotFound();
                }
                return Ok(utilisateur);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Search endpoint by nom, prenom, or email
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string nom, [FromQuery] string prenom, [FromQuery] string email)
        {
            // Get all users
            var utilisateurs = await _utilisateurService.GetAllAsync();

            // Filtering based on the provided parameters
            if (!string.IsNullOrEmpty(nom))
            {
                utilisateurs = utilisateurs.Where(u => u.nom.Contains(nom)).ToList();
            }

            if (!string.IsNullOrEmpty(prenom))
            {
                utilisateurs = utilisateurs.Where(u => u.prenom.Contains(prenom)).ToList();
            }

            if (!string.IsNullOrEmpty(email))
            {
                utilisateurs = utilisateurs.Where(u => u.email.Contains(email)).ToList();
            }

            // Return the filtered list of users (including all details for each user)
            if (utilisateurs.Any())
            {
                return Ok(utilisateurs); // All user information will be returned here
            }

            return NotFound("No users found matching the search criteria.");
        }
        
    }
}