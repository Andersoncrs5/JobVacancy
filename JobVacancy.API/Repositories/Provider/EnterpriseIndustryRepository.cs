using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class EnterpriseIndustryRepository(AppDbContext context, IRedisService redisService) 
    : GenericRepository<EnterpriseIndustryEntity>(context, redisService), IEnterpriseIndustryRepository
{
    public async Task<bool> ExistsByIndustryIdAndEnterpriseId(string industryId, string enterpriseId)
    {
        return await context.EnterpriseIndustries.AnyAsync(u => u.IndustryId == industryId && u.EnterpriseId == enterpriseId);
    }

    public async Task<EnterpriseIndustryEntity?> GetByIndustryIdAndEnterpriseId(string industryId, string enterpriseId)
    {
        return await context.EnterpriseIndustries
            .Where(u => u.EnterpriseId == enterpriseId && u.IndustryId == industryId)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetAmountIndustryByEnterpriseId(string enterpriseId)
    {
        return await context.EnterpriseIndustries.CountAsync(u => u.EnterpriseId == enterpriseId);
    }
    
}