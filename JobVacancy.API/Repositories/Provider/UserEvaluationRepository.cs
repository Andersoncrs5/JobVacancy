using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class UserEvaluationRepository(AppDbContext ctx) : GenericRepository<UserEvaluationEntity>(ctx),
    IUserEvaluationRepository
{
    public async Task<bool> ExistsByEnterpriseIdAndTargetUserId(string enterpriseId, string targetUserId)
        => await ctx.UserEvaluationEntities.AnyAsync(x => x.EnterpriseId == enterpriseId && x.TargetUserId == targetUserId);
    
    public async Task<UserEvaluationEntity?> GetByEnterpriseIdAndTargetUserId(string enterpriseId, string targetUserId)
        => await ctx.UserEvaluationEntities.FirstOrDefaultAsync(x => x.EnterpriseId == enterpriseId && x.TargetUserId == targetUserId);
}