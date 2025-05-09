using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PFE_PROJECT.Models;
using PFE_PROJECT.Repositories;

namespace PFE_PROJECT.Services
{
    public class UtilisateurService : IUtilisateurService
    {
        private readonly IUtilisateurRepository _utilisateurRepository;
        private readonly IRoleRepository _roleRepository;

        public UtilisateurService(IUtilisateurRepository utilisateurRepository, IRoleRepository roleRepository)
        {
            _utilisateurRepository = utilisateurRepository;
            _roleRepository = roleRepository;
        }

       // Renommer la méthode pour plus de clarté
public Utilisateur AuthenticateAdmin(string email, string motpasse)
{
    // Cette méthode retourne uniquement un utilisateur s'il a le rôle "Admin IT"
    var user = _utilisateurRepository.GetAdminUserByEmailAndPassword(email, motpasse);
    
    // Vérifier si l'utilisateur est désactivé
    if (user != null && user.Actif == "0")
    {
        throw new InvalidOperationException("Votre compte a été désactivé. Veuillez contacter l'administrateur.");
    }

    return user;
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
            {
                utilisateur.Actif = "1"; // Default to active
            }

            var roleExists = await _roleRepository.ExistsAsync(utilisateur.idrole);
            if (!roleExists)
            {
                throw new InvalidOperationException("Le rôle spécifié n'existe pas.");
            }

            var createdUser = await _utilisateurRepository.CreateAsync(utilisateur);
            return createdUser;
        }

        public async Task<Utilisateur> UpdateAsync(Utilisateur utilisateur)
{
    if (utilisateur == null)
    {
        throw new ArgumentNullException(nameof(utilisateur));
    }

    var existingUser = await _utilisateurRepository.GetByIdAsync(utilisateur.iduser);
    if (existingUser == null)
    {
        throw new InvalidOperationException($"L'utilisateur avec ID {utilisateur.iduser} n'existe pas.");
    }

    // Vérifier si le rôle existe
    if (utilisateur.idrole > 0)
    {
        var roleExists = await _roleRepository.ExistsAsync(utilisateur.idrole);
        if (!roleExists)
        {
            throw new InvalidOperationException($"Le rôle avec ID {utilisateur.idrole} n'existe pas.");
        }
    }

    // Mettre à jour l'utilisateur
    return await _utilisateurRepository.UpdateAsync(utilisateur);
}
        public async Task<Utilisateur> ActivateDeactivateAsync(int id, bool activate)
        {
            var utilisateur = await _utilisateurRepository.GetByIdAsync(id);
            if (utilisateur == null)
            {
                throw new InvalidOperationException("L'utilisateur spécifié n'existe pas.");
            }

            // Update the 'actif' field
            utilisateur.Actif = activate ? "1" : "0"; // "1" for active, "0" for deactivated

            return await _utilisateurRepository.UpdateAsync(utilisateur);
        }

        // ✅ NEW METHODS FOR AUTH LOGIC (Admin Métier, Responsable Unité, etc.)
        public async Task<Utilisateur> GetByEmailAsync(string email)
        {
            return await _utilisateurRepository.GetByEmailAsync(email);
        }

        public async Task<string> GetRoleNameByIdAsync(int idRole)
        {
            var role = await _roleRepository.GetByIdAsync(idRole);
            return role?.libelle;
        }

        // Method to verify if user is deactivated
        public async Task<Utilisateur> VerifyUserActiveStatus(string email)
        {
            var user = await _utilisateurRepository.GetByEmailAsync(email);
            if (user != null && user.Actif == "0")
            {
                throw new InvalidOperationException("Votre compte a été désactivé. Veuillez contacter l’administrateur.");
            }

            return user;
        }
    }
}
