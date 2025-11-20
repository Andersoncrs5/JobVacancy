using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;

namespace JobVacancy.API.Repositories.Provider;

public class PostEnterpriseRepository(AppDbContext context, IRedisService redisService) 
    : GenericRepository<PostEnterpriseEntity>(context, redisService), IPostEnterpriseRepository
{
    
}