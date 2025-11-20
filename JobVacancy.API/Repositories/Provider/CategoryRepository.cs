using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class CategoryRepository(AppDbContext context, IRedisService redisService) 
    : GenericRepository<CategoryEntity>(context, redisService), ICategoryRepository
{
    public async Task<bool> ExistsByName(string name)
    {
        return await Context.Categories
            .AnyAsync(c => c.Name == name);
    }
}