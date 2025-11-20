using JobVacancy.API.models.entities;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class UserContentReactionService(IUnitOfWork uow): IUserContentReactionService
{
    public async Task<bool> ExistsByUserIdAndContentIdAndReaction(
        string userId,
        string contentId,
        ReactionTypeEnum reactionType,
        ReactionTargetEnum targetType
    )
        => await uow.UserContentReactionRepository.ExistsByUserIdAndContentIdAndReaction(userId, contentId, reactionType, targetType);
    
    public async Task<UserContentReactionEntity?> GetByUserIdAndContentIdAndReaction(
        string userId,
        string contentId,
        ReactionTypeEnum reactionType,
        ReactionTargetEnum targetType
    )
        => await uow.UserContentReactionRepository.GetByUserIdAndContentIdAndReaction(userId, contentId, reactionType, targetType);

    public async Task<bool> ExistsById(string id) 
        => await uow.UserContentReactionRepository.ExistsById(id);
    
    public async Task<UserContentReactionEntity?> GetById(string id) 
        => await uow.UserContentReactionRepository.GetByIdAsync(id);

    public async Task Delete(UserContentReactionEntity contentReaction)
    {
        await uow.UserContentReactionRepository.DeleteAsync(contentReaction);
        await uow.Commit();
    }

    public async Task<UserContentReactionEntity> Update(UserContentReactionEntity contentReaction)
    {
        UserContentReactionEntity update = await uow.UserContentReactionRepository.Update(contentReaction);
        await uow.Commit();
        
        return update;
    }
    
    public async Task<UserContentReactionEntity> UpdateToggleReactionType(UserContentReactionEntity contentReaction)
    {
        if (contentReaction.ReactionType == ReactionTypeEnum.Like)
            contentReaction.ReactionType = ReactionTypeEnum.Dislike;
        
        if (contentReaction.ReactionType == ReactionTypeEnum.Dislike)
            contentReaction.ReactionType = ReactionTypeEnum.Like;
        
        UserContentReactionEntity update = await uow.UserContentReactionRepository.Update(contentReaction);
        await uow.Commit();
        
        return update;
    }
    
    public async Task<UserContentReactionEntity> Create(
        string userId, string contentId, 
        ReactionTypeEnum reactionType, ReactionTargetEnum targetType
        )
    {
        UserContentReactionEntity content = new UserContentReactionEntity()
        {
            UserId = userId,
            ReactionType = reactionType,
            TargetType = targetType,
        };
        
        switch (targetType)
        {
            case ReactionTargetEnum.PostUser:
                content.PostUserId = contentId;
                break;
            case ReactionTargetEnum.PostEnterprise:
                content.PostEnterpriseId = contentId;
                break;
            case ReactionTargetEnum.CommentUser: 
                content.CommentUserId = contentId;
                break;
            case ReactionTargetEnum.CommentEnterprise:
                content.CommentEnterpriseId = contentId;
                break;
            default:
                throw new ArgumentException($"Reaction target type '{targetType}' is unsupported or invalid.");
        }

        UserContentReactionEntity async = await uow.UserContentReactionRepository.AddAsync(content);
        await uow.Commit();
        return async;
    }
    
}