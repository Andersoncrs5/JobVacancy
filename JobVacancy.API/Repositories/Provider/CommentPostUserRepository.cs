using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;

namespace JobVacancy.API.Repositories.Provider;

public class CommentPostUserRepository(AppDbContext context): GenericRepository<CommentPostUserEntity>(context), ICommentPostUserRepository
{
    
}