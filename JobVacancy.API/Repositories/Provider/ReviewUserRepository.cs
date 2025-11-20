using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class ReviewUserRepository(AppDbContext ctx, IRedisService redisService): GenericRepository<ReviewUserEntity>(ctx, redisService), IReviewUserRepository
{
    public async Task<bool> ExistsByActorIdAndTargetId(string actorId, string targetId)
        => await ctx.ReviewUsers.AnyAsync(x => x.ActorId == actorId && x.TargetUserId == targetId);
}