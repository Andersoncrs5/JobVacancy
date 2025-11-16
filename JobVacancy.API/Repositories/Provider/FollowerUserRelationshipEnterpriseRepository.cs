using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class FollowerUserRelationshipEnterpriseRepository(AppDbContext ctx): GenericRepository<FollowerUserRelationshipEnterpriseEntity>(ctx),IFollowerUserRelationshipEnterpriseRepository
{
    public async Task<bool> ExistsByEnterpriseIdAndUserId(string enterpriseId, string userId)
        => await ctx.FollowerUserRelationshipEnterpriseEntities.AnyAsync(x => x.EnterpriseId == enterpriseId && x.UserId == userId);
    
    public async Task<FollowerUserRelationshipEnterpriseEntity?> GetByEnterpriseIdAndUserId(string enterpriseId, string userId)
        => await ctx.FollowerUserRelationshipEnterpriseEntities.FirstOrDefaultAsync(x => x.EnterpriseId == enterpriseId && x.UserId == userId);
}