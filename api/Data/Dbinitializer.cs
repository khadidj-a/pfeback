using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using PFE_PROJECT.Models;
using PFE_PROJECT.Data;

namespace PFE_PROJECT.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any roles
            if (context.Roles.Any())
            {
                return;   // DB has been seeded
            }

            var roles = new Role[]
            {
                new Role { libelle = "Admin IT" },
                new Role { libelle = "Administrateur Métier" },
                new Role { libelle = "Responsable Unité" }
            };

            foreach (var role in roles)
            {
                context.Roles.Add(role);
            }
            context.SaveChanges();

            // Create default admin user if none exists
            if (!context.Utilisateurs.Any(u => u.email == "admin@example.com"))
            {
                var adminUser = new Utilisateur
                {
                    nom = "Admin",
                    prenom = "User",
                    email = "admin@example.com",
                    motpasse = HashPassword("Admin123!"),
                    idrole = roles.First(r => r.libelle == "Admin IT").idrole,
                    idunite = 1, // Default unit ID
                    Actif = "1"
                };
                
                context.Utilisateurs.Add(adminUser);
                context.SaveChanges();
            }
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}