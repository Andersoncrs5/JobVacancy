
using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class UserRepository(AppDbContext context, UserManager<UserEntity> userManager): IUserRepository
{
    public async Task<UserEntity?> GetById(string id)
    {
        return await userManager.FindByIdAsync(id);
    }
    
    public async Task<UserEntity?> GetByEmail(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<bool> ExistsByEmail(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user != null;
    }

    public async Task<bool> ExistsByUsername(string username)
    {
        var user = await userManager.FindByNameAsync(username);
        return user != null;
    }
    
    public async Task<UserEntity?> GetByUsername(string username)
    {
        return await userManager.FindByNameAsync(username);
    }

    public async Task<IdentityResult> Delete(UserEntity user)
    {
        return await userManager.DeleteAsync(user);
    }

    public async Task<IdentityResult> Insert(UserEntity user)
    {
        return await userManager.CreateAsync(user, user.PasswordHash!);
    }

    public async Task<IdentityResult> Update(UserEntity user)
    {
        return await userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> AddRoleToUser(UserEntity user, RoleEntity role)
    {
        return await userManager.AddToRoleAsync(user, role.Name!);
    }

    public async Task<IList<string>> GetRolesAsync(UserEntity user)
    {
        IList<string> rolesAsync = await userManager.GetRolesAsync(user);
        return rolesAsync;
    }
    
}