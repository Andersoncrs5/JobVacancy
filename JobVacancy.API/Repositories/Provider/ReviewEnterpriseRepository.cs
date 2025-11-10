using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class ReviewEnterpriseRepository(AppDbContext context): GenericRepository<ReviewEnterpriseEntity>(context), IReviewEnterpriseRepository
{
    public async Task<bool> ExistsByUserIdAndEnterpriseId(string userId, string enterpriseId)
    {
        return await context.ReviewEnterpriseEntities.AnyAsync(x => x.UserId == userId && x.EnterpriseId == enterpriseId);
    }

    public async Task<ReviewEnterpriseEntity?> GetByUserIdAndEnterpriseId(string userId, string enterpriseId)
    {
        return await context.ReviewEnterpriseEntities.FirstOrDefaultAsync(x => x.UserId == userId && x.EnterpriseId == enterpriseId);
    }
}