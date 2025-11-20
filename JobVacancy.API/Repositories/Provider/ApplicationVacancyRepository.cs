using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class ApplicationVacancyRepository(
    AppDbContext context,
    IRedisService redisService
) : GenericRepository<ApplicationVacancyEntity>(context, redisService), 
    IApplicationVacancyRepository
{
    public async Task<bool> ExistsByVacancyIdAndUserId(string vacancyId, string userId)
        => await context.ApplicationVacancies
            .AnyAsync(x => x.VacancyId == vacancyId && x.UserId == userId);
    
    public async Task<ApplicationVacancyEntity?> GetByVacancyIdAndUserId(string vacancyId, string userId)
        => await context.ApplicationVacancies
            .FirstOrDefaultAsync(x => x.VacancyId == vacancyId && x.UserId == userId);
}
