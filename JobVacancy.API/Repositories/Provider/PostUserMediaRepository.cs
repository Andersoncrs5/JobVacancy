using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class PostUserMediaRepository(AppDbContext context, IRedisService redisService): GenericRepository<PostUserMediaEntity>(context, redisService),IPostUserMediaRepository
{
    public async Task<int> TotalMediaByPost(string postId)
        => await context.PostUserMediaEntities.CountAsync(x => x.PostId == postId);
}