
using Api.Context;
using Api.models.entities;
using Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

    public async Task<bool> ExistsByEmail(string email)
    {
        var result = await context.Users
            .Where(u => u.Email == email)
            .Select(u => u.Id)
            .FirstOrDefaultAsync();
        
        return result != null;
    }

    public async Task<bool> ExistsByUsername(string username)
    {
        var result = await context.Users
            .Where(u => u.UserName == username)
            .Select(u => u.Id)
            .SingleOrDefaultAsync();
        return result != null;
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
        return await userManager.CreateAsync(user);
    }

    public async Task<IdentityResult> Update(UserEntity user)
    {
        return await userManager.UpdateAsync(user);
    }
    
}