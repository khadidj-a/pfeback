using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PFE_PROJECT.DTOs;
using PFE_PROJECT.Helpers;
using PFE_PROJECT.Models;
using PFE_PROJECT.Repositories;

namespace PFE_PROJECT.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        
        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return roles.Select(MapToDto);
        }
        
        public async Task<RoleDto> GetRoleByIdAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                return null;
                
            return MapToDto(role);
        }
        
        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createDto)
        {
            // Check if libelle is one of the allowed roles
            if (!RoleHelper.IsValidRole(createDto.Libelle))
                throw new InvalidOperationException("Invalid role name. Allowed roles are: Admin IT, Administrateur Métier, Responsable Unité");
                
            // Check if libelle already exists
            if (await _roleRepository.LibelleExistsAsync(createDto.Libelle))
                throw new InvalidOperationException("Role with this name already exists");
                
            var role = new Role
            {
                libelle = createDto.Libelle
            };
            
            await _roleRepository.CreateAsync(role);
            
            return MapToDto(role);
        }
        
        public async Task<RoleDto> UpdateRoleAsync(int id, UpdateRoleDto updateDto)
        {
            // Check if libelle is one of the allowed roles
            if (!RoleHelper.IsValidRole(updateDto.Libelle))
                throw new InvalidOperationException("Invalid role name. Allowed roles are: Admin IT, Administrateur Métier, Responsable Unité");
                
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                throw new InvalidOperationException("Role not found");
                
            // Check if libelle already exists (but not for this role)
            if (updateDto.Libelle != role.libelle && 
                await _roleRepository.LibelleExistsAsync(updateDto.Libelle, id))
                throw new InvalidOperationException("Role with this name already exists");
                
            role.libelle = updateDto.Libelle;
            
            await _roleRepository.UpdateAsync(role);
            
            return MapToDto(role);
        }
        
        public async Task DeleteRoleAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                throw new InvalidOperationException("Role not found");
                
            // Don't allow deletion of the three main roles
            if (RoleHelper.IsValidRole(role.libelle))
                throw new InvalidOperationException("Cannot delete a system role");
                
            await _roleRepository.DeleteAsync(id);
        }
        
        private RoleDto MapToDto(Role role)
        {
            return new RoleDto
            {
                IdRole = role.idrole,
                Libelle = role.libelle
            };
        }
    }
}

