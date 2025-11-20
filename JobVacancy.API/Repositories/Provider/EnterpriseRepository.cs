using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class EnterpriseRepository(AppDbContext context, IRedisService redisService) 
    : GenericRepository<EnterpriseEntity>(context, redisService), IEnterpriseRepository
{
    public async Task<bool> ExistsByName(string name)
    {
        return await context.Enterprises.AnyAsync(u => u.Name == name);
    }
    
    public async Task<bool> ExistsByUserId(string userId)
    {
        return await context.Enterprises.AnyAsync(u => u.UserId == userId);
    }
    
    public async Task<EnterpriseEntity?> GetByUserId(string userId)
    {
        return await context.Enterprises.Where(u => u.UserId == userId).FirstOrDefaultAsync();
    }
    
}