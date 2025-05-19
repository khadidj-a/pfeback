using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PFE_PROJECT.DTOs;
using PFE_PROJECT.Services;
using PFE_PROJECT.Models;
using System.Threading.Tasks;

namespace PFE_PROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUtilisateurService _utilisateurService;
        private readonly IJwtService _jwtService;

        public AuthController(
            IUtilisateurService utilisateurService, 
            IJwtService jwtService)
        {
            _utilisateurService = utilisateurService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _utilisateurService.GetByEmailAsync(loginDto.Email);

            if (user == null || !_utilisateurService.VerifyPassword(user.motpasse, loginDto.motpasse))
                return Unauthorized("❌ Email ou mot de passe incorrect.");

            if (user.Actif == "0")
                return Unauthorized("⛔ Votre compte est désactivé. Contactez l’administrateur.");

            var roleName = await _utilisateurService.GetRoleNameByIdAsync(user.idrole);

            if (roleName != "Admin IT" && roleName != "Admin Métier" && roleName != "Responsable Unité")
                return Forbid("⛔ Accès refusé : rôle non autorisé.");

            var token = _jwtService.GenerateToken(user, roleName);

            var response = new LoginResponseDto
            {
                IdUser = user.iduser,
                Nom = user.nom,
                Prenom = user.prenom,
                Email = user.email,
                Idrole = user.idrole,
                Libelle = roleName,
                IsAdmin = roleName == "Admin IT" || roleName == "Admin Métier"
            };

            return Ok(new
            {
                token,
                user = response
            });
        }
    }
}
