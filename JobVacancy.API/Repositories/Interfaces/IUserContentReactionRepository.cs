using JobVacancy.API.models.entities;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IUserContentReactionRepository: IGenericRepository<UserContentReactionEntity>
{
    Task<bool> ExistsByUserIdAndContentIdAndReaction(
        string userId, string contentId, ReactionTargetEnum targetType
    );

    Task<UserContentReactionEntity?> GetByUserIdAndContentIdAndReaction(
        string userId,
        string contentId, 
        ReactionTargetEnum targetType
    );

}