using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class UserContentReactionRepository(AppDbContext context, IRedisService redisService): GenericRepository<UserContentReactionEntity>(context, redisService), IUserContentReactionRepository
{
    private IQueryable<UserContentReactionEntity> _BuildReactionQuery(
        string userId,
        string contentId,
        ReactionTargetEnum targetType)
    {
        var query = context.UserContentReactionEntities
            .Where(x => x.UserId == userId && 
                        x.TargetType == targetType);
        
        switch (targetType)
        {
            case ReactionTargetEnum.PostUser:
                query = query.Where(x => x.PostUserId == contentId);
                break;
            case ReactionTargetEnum.PostEnterprise:
                query = query.Where(x => x.PostEnterpriseId == contentId);
                break;
            case ReactionTargetEnum.CommentUser: 
                query = query.Where(x => x.CommentUserId == contentId);
                break;
            case ReactionTargetEnum.CommentEnterprise:
                query = query.Where(x => x.CommentEnterpriseId == contentId);
                break;
            default:
                throw new ArgumentException($"Reaction target type '{targetType}' is unsupported or invalid.");
        }

        return query;
    }
    
    public async Task<bool> ExistsByUserIdAndContentIdAndReaction(
        string userId,
        string contentId,
        ReactionTargetEnum targetType
    )
    {
        var query = _BuildReactionQuery(userId, contentId, targetType);
        
        return await query.AnyAsync();
    }
    
    public async Task<UserContentReactionEntity?> GetByUserIdAndContentIdAndReaction(
        string userId,
        string contentId,
        ReactionTargetEnum targetType
    )
    {
        IQueryable<UserContentReactionEntity> query = _BuildReactionQuery(userId, contentId, targetType);

        return await query.FirstOrDefaultAsync();
    }  
    
}