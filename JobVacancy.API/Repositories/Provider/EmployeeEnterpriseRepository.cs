using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class EmployeeEnterpriseRepository(AppDbContext context, IRedisService redisService): GenericRepository<EmployeeEnterpriseEntity>(context, redisService), IEmployeeEnterpriseRepository
{
    public async Task<bool> ExistsByUserIdAndEnterpriseId(string userId, string enterpriseId)
    {
        return await context.EmployeeEnterprises.AnyAsync(x => x.UserId == userId && x.EnterpriseId == enterpriseId);
    }

    public async Task<EmployeeEnterpriseEntity?> GetByUserIdAndEnterpriseId(string userId, string enterpriseId)
    {
        return await context.EmployeeEnterprises.FirstOrDefaultAsync(x => x.UserId == userId && x.EnterpriseId == enterpriseId);
    }
}