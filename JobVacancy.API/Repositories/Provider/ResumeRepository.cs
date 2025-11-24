using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class ResumeRepository(AppDbContext ctx, IRedisService redisService): GenericRepository<ResumeEntity>(ctx, redisService), IResumeRepository
{
    public async Task<bool> ExistsByName(string name)
        => await ctx.ResumeEntities.AnyAsync(x => EF.Functions.Like(x.Name, $"%{name}%"));
    
    public async Task<ResumeEntity?> GetByName(string name)
        => await ctx.ResumeEntities.FirstOrDefaultAsync(x => EF.Functions.Like(x.Name, $"%{name}%"));
    
    public async Task<bool> ExistsByObjectKey(string key)
        => await ctx.ResumeEntities.AnyAsync(x => EF.Functions.Like(x.ObjectKey, $"%{key}%"));
    
    public async Task<ResumeEntity?> GetByObjectKey(string key)
        => await ctx.ResumeEntities.FirstOrDefaultAsync(x => EF.Functions.Like(x.ObjectKey, $"%{key}%"));
    
    public async Task<bool> ExistsByUrl(string url)
        => await ctx.ResumeEntities.AnyAsync(x => EF.Functions.Like(x.Url, $"%{url}%"));
    
    public async Task<ResumeEntity?> GetByUrl(string url)
        => await ctx.ResumeEntities.FirstOrDefaultAsync(x => EF.Functions.Like(x.Url, $"%{url}%"));
}