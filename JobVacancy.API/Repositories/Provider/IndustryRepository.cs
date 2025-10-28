using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class IndustryRepository(AppDbContext context) 
    : GenericRepository<IndustryEntity>(context), IIndustryRepository
{
    public async Task<bool> ExistsByName(string name)
    {
        return await Context.Industries
            .AnyAsync(c => c.Name == name);
    }
}