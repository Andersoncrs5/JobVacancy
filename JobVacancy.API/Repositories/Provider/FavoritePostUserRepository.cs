using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class FavoritePostUserRepository(AppDbContext context, IRedisService redisService): GenericRepository<FavoritePostUserEntity>(context, redisService), IFavoritePostUserRepository
{
    public async Task<FavoritePostUserEntity?> GetByUserIdAndPostUserId(string userId, string postUserId)
    {
        return await context.FavoritePostUser.FirstOrDefaultAsync(e => e.UserId == userId && e.PostUserId == postUserId);
    } 
    
    public async Task<bool> ExistsByUserIdAndPostUserId(string userId, string postUserId)
    {
        return await context.FavoritePostUser.AnyAsync(e => e.UserId == userId && e.PostUserId == postUserId);
    }
    
    public async Task<FavoritePostUserEntity?> GetByUserIdAndPostUserIdWithEntity(string userId, string postUserId)
    {
        IQueryable<FavoritePostUserEntity> queryable = context.FavoritePostUser.AsQueryable();
        queryable = queryable.Include(e => e.PostUser);
        queryable = queryable.Include(e => e.User);
        return await queryable.FirstOrDefaultAsync(e => e.UserId == userId && e.PostUserId == postUserId);
    } 
    
}