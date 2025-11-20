using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class PositionRepository(AppDbContext context, IRedisService redisService): GenericRepository<PositionEntity>(context, redisService), IPositionRepository
{
    public async Task<bool> ExistsByName(string name)
    {
        return await context.Positions.AnyAsync(x => x.Name == name);
    }
}