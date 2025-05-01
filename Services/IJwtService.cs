using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface IJwtService
    {
        string GenerateToken(Utilisateur user, string roleLibelle);
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Utilisateur user, string roleLibelle)
        {
            // ✅ Read key from config
            var key = _configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(key) || key.Length < 32)
            {
                throw new InvalidOperationException("La clé JWT doit contenir au moins 32 caractères");
            }

            // ✅ Build claims
            var claims = new[]
            {  

                new Claim(ClaimTypes.NameIdentifier, user.iduser.ToString()),
                new Claim(ClaimTypes.Name, $"{user.nom} {user.prenom}"),
                new Claim(ClaimTypes.Email, user.email),
                new Claim(ClaimTypes.Role, roleLibelle), // 👈 inject dynamic role here
                new Claim("idunite", user.idunite.ToString())

            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            // ⏰ Expiration time
            double expireMinutes = 180; // default 3 hours
            if (double.TryParse(_configuration["Jwt:ExpireMinutes"], out double configExpireMinutes))
            {
                expireMinutes = configExpireMinutes;
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
