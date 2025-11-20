using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class FollowerUserRelationshipEnterpriseRepository(AppDbContext ctx, IRedisService redisService): GenericRepository<FollowerUserRelationshipEnterpriseEntity>(ctx, redisService),IFollowerUserRelationshipEnterpriseRepository
{
    public async Task<bool> ExistsByEnterpriseIdAndUserId(string enterpriseId, string userId)
        => await ctx.FollowerUserRelationshipEnterpriseEntities.AnyAsync(x => x.EnterpriseId == enterpriseId && x.UserId == userId);
    
    public async Task<FollowerUserRelationshipEnterpriseEntity?> GetByEnterpriseIdAndUserId(string enterpriseId, string userId)
        => await ctx.FollowerUserRelationshipEnterpriseEntities.FirstOrDefaultAsync(x => x.EnterpriseId == enterpriseId && x.UserId == userId);
}