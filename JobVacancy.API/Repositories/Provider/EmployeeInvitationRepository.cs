using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;

namespace JobVacancy.API.Repositories.Provider;

public class EmployeeInvitationRepository(AppDbContext context, IRedisService redisService) 
    :GenericRepository<EmployeeInvitationEntity>(context, redisService), IEmployeeInvitationRepository
{
    
}