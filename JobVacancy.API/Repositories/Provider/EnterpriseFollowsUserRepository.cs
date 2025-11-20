using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class EnterpriseFollowsUserRepository(AppDbContext context, IRedisService redisService): GenericRepository<EnterpriseFollowsUserEntity>(context, redisService), IEnterpriseFollowsUserRepository
{
    public async Task<bool> ExistsByEnterpriseIdAndUserId(string enterpriseId, string userId)
        => await context.EnterpriseFollowsUserEntities.AnyAsync(x => x.EnterpriseId == enterpriseId && x.UserId == userId);
    
    public async Task<EnterpriseFollowsUserEntity?> GetByEnterpriseIdAndUserId(string enterpriseId, string userId)
        => await context.EnterpriseFollowsUserEntities.FirstOrDefaultAsync(x => x.EnterpriseId == enterpriseId && x.UserId == userId);
}