using Api.models.dtos.Role;
using Api.models.entities;

namespace Api.Services.Interfaces;

public interface IRolesService
{
    Task<RoleResult> CreateRole(RoleEntity role);
    Task<RoleEntity?> GetById(string id);
    Task<RoleEntity?> GetByName(string name);
    Task<IEnumerable<RoleEntity>> GetAllRoles();
    
}