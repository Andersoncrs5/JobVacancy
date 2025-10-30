using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;

namespace JobVacancy.API.Repositories.Provider;

public class PostUserRepository(AppDbContext context) 
    : GenericRepository<PostUserEntity>(context), IPostUserRepository
{
    
}