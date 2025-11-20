using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class UserEvaluationRepository(AppDbContext ctx, IRedisService redisService) : GenericRepository<UserEvaluationEntity>(ctx, redisService),
    IUserEvaluationRepository
{
    public async Task<bool> ExistsByEnterpriseIdAndTargetUserId(string enterpriseId, string targetUserId)
        => await ctx.UserEvaluationEntities.AnyAsync(x => x.EnterpriseId == enterpriseId && x.TargetUserId == targetUserId);
    
    public async Task<UserEvaluationEntity?> GetByEnterpriseIdAndTargetUserId(string enterpriseId, string targetUserId)
        => await ctx.UserEvaluationEntities.FirstOrDefaultAsync(x => x.EnterpriseId == enterpriseId && x.TargetUserId == targetUserId);
}