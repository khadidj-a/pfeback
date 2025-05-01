using System.Collections.Generic;
using System.Threading.Tasks;
using PFE_PROJECT.DTOs;

namespace PFE_PROJECT.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(int id);
        Task<RoleDto> CreateRoleAsync(CreateRoleDto createDto);
        Task<RoleDto> UpdateRoleAsync(int id, UpdateRoleDto updateDto);
        Task DeleteRoleAsync(int id);
    }
}

