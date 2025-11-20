using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class IndustryRepository (AppDbContext context, IRedisService redisService)
    : GenericRepository<IndustryEntity>(context, redisService), IIndustryRepository
{
    public async Task<bool> ExistsByName(string name)
        => await Context.Industries.AnyAsync(c => c.Name == name);
    
}
