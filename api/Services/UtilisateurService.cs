using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PFE_PROJECT.Models;
using PFE_PROJECT.Repositories;

namespace PFE_PROJECT.Services
{
    public class UtilisateurService : IUtilisateurService
    {
        private readonly IUtilisateurRepository _utilisateurRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UtilisateurService(
            IUtilisateurRepository utilisateurRepository,
            IRoleRepository roleRepository,
            IPasswordHasher passwordHasher)
        {
            _utilisateurRepository = utilisateurRepository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Utilisateur?> AuthenticateAdminAsync(string email, string motpasse)
        {
            var user = await _utilisateurRepository.GetByEmailAsync(email);
            if (user == null) return null;

            if (user.Actif == "0")
                throw new InvalidOperationException("Votre compte a été désactivé. Veuillez contacter l'administrateur.");

            if (_passwordHasher.VerifyPassword(user.motpasse, motpasse))
                return user;

            return null;
        }

        public async Task<List<Utilisateur>> GetAllAsync()
        {
            return await _utilisateurRepository.GetAllAsync();
        }

        public async Task<Utilisateur> GetByIdAsync(int id)
        {
            return await _utilisateurRepository.GetByIdAsync(id);
        }

        public async Task<Utilisateur> CreateAsync(Utilisateur utilisateur)
        {
            if (string.IsNullOrEmpty(utilisateur.Actif))
                utilisateur.Actif = "1";

            var roleExists = await _roleRepository.ExistsAsync(utilisateur.idrole);
            if (!roleExists)
                throw new InvalidOperationException("Le rôle spécifié n'existe pas.");

            utilisateur.motpasse = _passwordHasher.HashPassword(utilisateur.motpasse);

            return await _utilisateurRepository.CreateAsync(utilisateur);
        }

        public async Task<Utilisateur> UpdateAsync(Utilisateur utilisateur)
        {
            if (utilisateur == null)
                throw new ArgumentNullException(nameof(utilisateur));

            var existingUser = await _utilisateurRepository.GetByIdAsync(utilisateur.iduser);
            if (existingUser == null)
                throw new InvalidOperationException($"L'utilisateur avec ID {utilisateur.iduser} n'existe pas.");

            if (utilisateur.idrole > 0)
            {
                var roleExists = await _roleRepository.ExistsAsync(utilisateur.idrole);
                if (!roleExists)
                    throw new InvalidOperationException($"Le rôle avec ID {utilisateur.idrole} n'existe pas.");
            }

            // Hash password only if changed
            if (utilisateur.motpasse != existingUser.motpasse)
            {
                utilisateur.motpasse = _passwordHasher.HashPassword(utilisateur.motpasse);
            }

            return await _utilisateurRepository.UpdateAsync(utilisateur);
        }

        public async Task<Utilisateur> ActivateDeactivateAsync(int id, bool activate)
        {
            var utilisateur = await _utilisateurRepository.GetByIdAsync(id);
            if (utilisateur == null)
                throw new InvalidOperationException("L'utilisateur spécifié n'existe pas.");

            utilisateur.Actif = activate ? "1" : "0";
            return await _utilisateurRepository.UpdateAsync(utilisateur);
        }

        public async Task<Utilisateur?> GetByEmailAsync(string email)
        {
            return await _utilisateurRepository.GetByEmailAsync(email);
        }

        public async Task<string?> GetRoleNameByIdAsync(int idRole)
        {
            var role = await _roleRepository.GetByIdAsync(idRole);
            return role?.libelle;
        }

        public async Task<Utilisateur?> VerifyUserActiveStatus(string email)
        {
            var user = await _utilisateurRepository.GetByEmailAsync(email);
            if (user != null && user.Actif == "0")
                throw new InvalidOperationException("Votre compte a été désactivé. Veuillez contacter l'administrateur.");

            return user;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var utilisateurs = await GetAllAsync();
            return utilisateurs.Any(user => user.email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        // ✅ Missing method now implemented:
        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            return _passwordHasher.VerifyPassword(hashedPassword, providedPassword);
        } 
        public async Task<int> GetUtilisateurCountAsync()
{
   return await _utilisateurRepository.GetUtilisateurCountAsync(); // ✅ Correct

}
    }   
}
