using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;

namespace JobVacancy.API.Repositories.Provider;

public class UserContentReactionRepository(AppDbContext context): GenericRepository<UserContentReactionEntity>(context), IUserContentReactionRepository
{
    
}