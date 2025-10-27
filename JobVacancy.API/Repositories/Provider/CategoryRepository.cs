using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class CategoryRepository(AppDbContext context) 
    : GenericRepository<CategoryEntity>(context), ICategoryRepository
{
    public async Task<bool> ExistsByName(string name)
    {
        return await Context.Categories
            .AnyAsync(c => c.Name == name);
    }
}