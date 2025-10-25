using JobVacancy.API.models.entities;
using Microsoft.AspNetCore.Identity;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<IEnumerable<RoleEntity>> GetAllAsync();
    Task<RoleEntity?> GetByIdAsync(string id);
    Task<RoleEntity?> GetByNameAsync(string name);
    Task<IdentityResult> CreateAsync(RoleEntity role);
    Task<IdentityResult> UpdateAsync(RoleEntity role);
    Task<IdentityResult> DeleteAsync(RoleEntity role);
}