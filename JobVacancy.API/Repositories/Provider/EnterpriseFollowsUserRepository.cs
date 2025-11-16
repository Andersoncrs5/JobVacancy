using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class EnterpriseFollowsUserRepository(AppDbContext ctx): GenericRepository<EnterpriseFollowsUserEntity>(ctx), IEnterpriseFollowsUserRepository
{
    public async Task<bool> ExistsByEnterpriseIdAndUserId(string enterpriseId, string userId)
        => await ctx.EnterpriseFollowsUserEntities.AnyAsync(x => x.EnterpriseId == enterpriseId && x.UserId == userId);
    
    public async Task<EnterpriseFollowsUserEntity?> GetByEnterpriseIdAndUserId(string enterpriseId, string userId)
        => await ctx.EnterpriseFollowsUserEntities.FirstOrDefaultAsync(x => x.EnterpriseId == enterpriseId && x.UserId == userId);
}