using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class FavoritePostEnterpriseRepository(AppDbContext context, IRedisService redisService): GenericRepository<FavoritePostEnterpriseEntity>(context, redisService), IFavoritePostEnterpriseRepository
{
    
    public async Task<FavoritePostEnterpriseEntity?> GetByUserIdAndPostId(string userId, string postId)
    {
        return await context.FavoritePostEnterprise.FirstOrDefaultAsync(e => e.UserId == userId && e.PostEnterpriseId == postId);
    } 
    
    public async Task<bool> ExistsByUserIdAndPostId(string userId, string postId)
    {
        return await context.FavoritePostEnterprise.AnyAsync(e => e.UserId == userId && e.PostEnterpriseId == postId);
    }

    public async Task<FavoritePostEnterpriseEntity?> GetByUserIdAndPostIdWithEntity(string userId, string postId)
    {
        IQueryable<FavoritePostEnterpriseEntity> queryable = context.FavoritePostEnterprise.AsQueryable();
        queryable = queryable.Include(e => e.PostEnterprise);
        queryable = queryable.Include(e => e.User);
        return await queryable.FirstOrDefaultAsync(e => e.UserId == userId && e.PostEnterpriseId == postId);
    }
}