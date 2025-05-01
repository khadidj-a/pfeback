using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public class MarqueService : IMarqueService
    {
        private readonly ApplicationDbContext _context;

        public MarqueService(ApplicationDbContext context)
        {
            _context = context;
        }

   public async Task<IEnumerable<MarqueDTO>> GetAllMarquesAsync(
    string? searchTerm = null,
    string? sortBy = null,
    bool ascending = true)
{
    Console.WriteLine($"🔍 Recherche: {searchTerm ?? "aucune"} | Tri: {sortBy ?? "idmarque"} | Ordre: {(ascending ? "Ascendant" : "Descendant")}");

    var query = _context.Marques.AsQueryable();

    if (!string.IsNullOrEmpty(searchTerm))
    {
        var lowerSearch = searchTerm.ToLower();
        query = query.Where(m =>
            (m.nom_fabriquant.ToLower().Contains(lowerSearch)) ||
            (m.codemarque != null && m.codemarque.ToLower().Contains(lowerSearch))
        );
    }

    sortBy ??= "idmarque"; // Tri par défaut

    query = sortBy.ToLower() switch
    {
        "codemarque" => ascending ? query.OrderBy(m => m.codemarque) : query.OrderByDescending(m => m.codemarque),
        "nom_fabriquant" => ascending ? query.OrderBy(m => m.nom_fabriquant) : query.OrderByDescending(m => m.nom_fabriquant),
        _ => ascending ? query.OrderBy(m => m.idmarque) : query.OrderByDescending(m => m.idmarque)
    };

    var marques = await query.ToListAsync();

    return marques.Select(m => new MarqueDTO
    {
        idmarque = m.idmarque,
        codemarque = m.codemarque ?? "",
        nom_fabriquant = m.nom_fabriquant
    });
}

        public async Task<MarqueDTO?> GetMarqueByIdAsync(int id)
        {
            var marque = await _context.Marques.FirstOrDefaultAsync(m => m.idmarque == id);
            return marque is null ? null : new MarqueDTO
            {
                idmarque = marque.idmarque,
                codemarque = marque.codemarque ?? "",
                nom_fabriquant = marque.nom_fabriquant
            };
        }

     public async Task<MarqueDTO> CreateMarqueAsync(CreateMarqueDTO marqueDto)
{
    try
    {
        // Créer une nouvelle marque avec seulement le nom du fabricant
        var marque = new Marque
        {
            nom_fabriquant = marqueDto.nom_fabriquant
        };

        // Ajouter à la base de données
        _context.Marques.Add(marque);
        await _context.SaveChangesAsync();

        // Récupérer la marque complète avec la colonne calculée
        var createdMarque = await _context.Marques.FindAsync(marque.idmarque);
        
        // Ligne 81 (dans UpdateMarqueAsync)
return new MarqueDTO
{
    idmarque = marque.idmarque,
    codemarque = marque.codemarque ?? "",
    nom_fabriquant = marque.nom_fabriquant
};

    }
    catch (Exception ex)
    {
        // Journaliser l'erreur
        Console.WriteLine($"Erreur dans CreateMarqueAsync: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
        }
        throw;
    }
}

        public async Task<MarqueDTO?> UpdateMarqueAsync(int id, UpdateMarqueDTO marqueDto)
        {
            var marque = await _context.Marques.FirstOrDefaultAsync(m => m.idmarque == id);
            if (marque is null) return null;

            marque.nom_fabriquant = marqueDto.nom_fabriquant;

            await _context.SaveChangesAsync();
            await _context.Entry(marque).ReloadAsync();

            return new MarqueDTO
            {
                idmarque = marque.idmarque,
                codemarque = marque.codemarque ?? "",
                nom_fabriquant = marque.nom_fabriquant
            };
        }

        public async Task<bool> CanDeleteMarqueAsync(int id)
        {
            return !await _context.Equipements.AnyAsync(e => e.idMarq == id);
        }

        public async Task<bool> DeleteMarqueAsync(int id)
        {
            if (!await CanDeleteMarqueAsync(id))
                return false;

            var marque = await _context.Marques.FirstOrDefaultAsync(m => m.idmarque == id);
            if (marque is null)
                return false;

            _context.Marques.Remove(marque);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

