using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;

namespace JobVacancy.API.Repositories.Provider;

public class VacancyRepository(AppDbContext context, IRedisService redisService): GenericRepository<VacancyEntity>(context, redisService), IVacancyRepository
{
    
}