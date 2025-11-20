using JobVacancy.API.models.entities;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.Services.Interfaces;

public interface IUserContentReactionService
{
    Task<UserContentReactionEntity> Create(
        string userId, string contentId,
        ReactionTypeEnum reactionType, ReactionTargetEnum targetType
    );
    Task<UserContentReactionEntity> UpdateToggleReactionType(UserContentReactionEntity contentReaction);
    Task<UserContentReactionEntity> Update(UserContentReactionEntity contentReaction);
    Task Delete(UserContentReactionEntity contentReaction);
    Task<bool> ExistsById(string id);
    Task<UserContentReactionEntity?> GetById(string id);

    Task<bool> ExistsByUserIdAndContentIdAndReaction(
        string userId,
        string contentId,
        ReactionTypeEnum reactionType,
        ReactionTargetEnum targetType
    );

    Task<UserContentReactionEntity?> GetByUserIdAndContentIdAndReaction(
        string userId,
        string contentId,
        ReactionTypeEnum reactionType,
        ReactionTargetEnum targetType
    );
}