
using Api.Context;
using Api.models.entities;
using Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Api.Repositories.Provider;

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

    public async Task<UserEntity?> GetByUsername(string username)
    {
        return await userManager.FindByNameAsync(username);
    }

    public async Task Delete(UserEntity user)
    {
        await userManager.DeleteAsync(user);
    }

    public async Task<IdentityResult> Insert(UserEntity user)
    {
        return await userManager.CreateAsync(user);
    }

    public async Task<IdentityResult> Update(UserEntity user)
    {
        return await userManager.UpdateAsync(user);
    }
    
}