using PFE_PROJECT.Models;

public interface IGroupeIdentiqueService
{
    Task<IEnumerable<GroupeIdentiqueDTO>> GetAllAsync(
    string? searchTerm = null,
    string? sortBy = null,
    bool ascending = true);

    Task<GroupeIdentiqueDTO?> GetByIdAsync(int id);
    Task<GroupeDetailsDTO?> GetGroupDetailsAsync(int id);
    Task<GroupeIdentiqueDTO?> UpdateAsync(int id, UpdateGroupeIdentiqueDTO dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> CanDeleteGroupeAsync(int id);
    Task<int> GetGroupeCountAsync();

}



