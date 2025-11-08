using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class PositionRepository(AppDbContext context): GenericRepository<PositionEntity>(context), IPositionRepository
{
    public async Task<bool> ExistsByName(string name)
    {
        return await context.Positions.AnyAsync(x => x.Name == name);
    }
}