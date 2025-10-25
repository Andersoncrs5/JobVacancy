using JobVacancy.API.models.dtos.Role;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IRolesService
{
    Task<RoleResult> CreateRole(RoleEntity role);
    Task<RoleEntity?> GetById(string id);
    Task<RoleEntity?> GetByName(string name);
    Task<IEnumerable<RoleEntity>> GetAllRoles();
    
}