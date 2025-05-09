using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using PFE_PROJECT.DTOs;
using PFE_PROJECT.Services;
using Microsoft.AspNetCore.Authorization;

namespace PFE_PROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        
        // GET: api/Roles
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }
        
        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            
            if (role == null)
                return NotFound();
                
            return Ok(role);
        }
        
        // POST: api/Roles
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleDto createDto)
        {
            try
            {
                var role = await _roleService.CreateRoleAsync(createDto);
                return CreatedAtAction(nameof(GetRole), new { id = role.IdRole }, role);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, UpdateRoleDto updateDto)
        {
            try
            {
                var role = await _roleService.UpdateRoleAsync(id, updateDto);
                return Ok(role);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "Role not found")
                    return NotFound();
                    
                return BadRequest(ex.Message);
            }
        }
        
        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                await _roleService.DeleteRoleAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "Role not found")
                    return NotFound();
                    
                return BadRequest(ex.Message);
            }
        }
    }
}