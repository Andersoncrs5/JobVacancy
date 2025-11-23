using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Provider;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class PostUserMetricsRepository(AppDbContext ctx, IRedisService redisService): GenericRepository<PostUserMetricsEntity>(ctx, redisService), IPostUserMetricsRepository
{
    public async Task<PostUserMetricsEntity?> GetByPostId(string postId)
        => await ctx.PostUserMetricsEntities.FirstOrDefaultAsync(x => x.PostId == postId);
    
    public async Task<bool> ExistsByPostId(string postId)
        => await ctx.PostUserMetricsEntities.AnyAsync(x => x.PostId == postId);
}