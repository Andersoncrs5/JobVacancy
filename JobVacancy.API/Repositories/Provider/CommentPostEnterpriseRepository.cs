using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;

namespace JobVacancy.API.Repositories.Provider;

public class CommentPostEnterpriseRepository(AppDbContext context): GenericRepository<CommentPostEnterpriseEntity>(context), ICommentPostEnterpriseRepository
{
    
}