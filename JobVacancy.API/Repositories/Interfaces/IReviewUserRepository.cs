using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IReviewUserRepository: IGenericRepository<ReviewUserEntity>
{
    Task<bool> ExistsByActorIdAndTargetId(string actorId, string targetId);
}