using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class AreaRepository(AppDbContext context): GenericRepository<AreaEntity>(context), IAreaRepository
{
    public async Task<bool> ExistsByName(string name)
    {
        return await context.AreaEntities.AsNoTracking().AnyAsync(x => x.Name == name);
    }

    public async Task<AreaEntity?> GetByName(string name)
    {
        return await context.AreaEntities.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name);
    }
    
}