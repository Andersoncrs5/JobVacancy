using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class RoleRepository(RoleManager<RoleEntity> roleManager) : IRoleRepository
{
    public async Task<IEnumerable<RoleEntity>> GetAllAsync()
        => await roleManager.Roles.ToListAsync();

    public async Task<RoleEntity?> GetByIdAsync(string id)
        => await roleManager.FindByIdAsync(id);

    public async Task<RoleEntity?> GetByNameAsync(string name)
        => await roleManager.FindByNameAsync(name);

    public async Task<IdentityResult> CreateAsync(RoleEntity role)
        => await roleManager.CreateAsync(role);

    public async Task<IdentityResult> UpdateAsync(RoleEntity role)
        => await roleManager.UpdateAsync(role);

    public async Task<IdentityResult> DeleteAsync(RoleEntity role)
        => await roleManager.DeleteAsync(role);
        
}