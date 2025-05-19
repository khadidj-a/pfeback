using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;
using PFE_PROJECT.Repositories; 

public class UtilisateurRepository : IUtilisateurRepository
{
    private readonly ApplicationDbContext _context;

    public UtilisateurRepository(ApplicationDbContext context)
    {
        _context = context;
    }

   public Utilisateur GetAdminUserByEmailAndPassword(string email, string motpasse)
{
    // D'abord, trouver l'ID du rôle "Admin IT"
    var adminRoleId = _context.Roles
        .Where(r => r.libelle == "Admin IT")
        .Select(r => r.idrole)
        .FirstOrDefault();

    if (adminRoleId == 0) // Si aucun rôle "Admin IT" n'est trouvé
        return null;

    // Ensuite, vérifier si l'utilisateur avec cet email et mot de passe a ce rôle
    return _context.Utilisateurs
        .Include(u => u.Role)
        .FirstOrDefault(u => 
            u.email == email && 
            u.motpasse == motpasse && 
            u.idrole == adminRoleId);
}


    public async Task<List<Utilisateur>> GetAllAsync()
{
    return await _context.Utilisateurs.Include(u => u.Role).ToListAsync();
}

public async Task<Utilisateur?> GetByIdAsync(int id)
{
    return await _context.Utilisateurs.Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.iduser == id);
}

public async Task<Utilisateur> CreateAsync(Utilisateur utilisateur)
{
    _context.Utilisateurs.Add(utilisateur);
    await _context.SaveChangesAsync();
    return utilisateur;
}

public async Task<Utilisateur> UpdateAsync(Utilisateur utilisateur)
{
    _context.Utilisateurs.Update(utilisateur);
    await _context.SaveChangesAsync();
    return utilisateur;
}

// public async Task DeleteAsync(int id)
// {
//     var utilisateur = await _context.Utilisateurs.FindAsync(id);
//     if (utilisateur != null)
//     {
//         _context.Utilisateurs.Remove(utilisateur);
//         await _context.SaveChangesAsync();
//     }
// }

public async Task<Utilisateur?> ActivateDeactivateAsync(int id, bool activate)
{
    var user = await _context.Utilisateurs.FindAsync(id);
    if (user == null) return null;

    user.Actif = activate ? "1" : "0";
    await _context.SaveChangesAsync();
    return user;
}


public async Task<Utilisateur> GetByEmailAsync(string email)
{
    return await _context.Utilisateurs
        .Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.email == email);
}
public async Task<int> GetUtilisateurCountAsync()
{
    return await _context.Utilisateurs.CountAsync();
}



}

