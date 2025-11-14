using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Utils.Uow.Provider;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class FollowerRelationshipUserRepository(AppDbContext context): GenericRepository<FollowerRelationshipUserEntity>(context), IFollowerRelationshipUserRepository
{
    public async Task<bool> ExistsByFollowerIdAndFollowedId(string followerId, string followedId)
        => await context.FollowerRelationshipUsers.AnyAsync(x => x.FollowerId == followerId && x.FollowedId == followedId);
    
    public async Task<FollowerRelationshipUserEntity?> GetByFollowerIdAndFollowedId(string followerId, string followedId)
        => await context.FollowerRelationshipUsers.FirstOrDefaultAsync(x => x.FollowerId == followerId && x.FollowedId == followedId);
    
}