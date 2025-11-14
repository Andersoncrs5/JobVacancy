using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IFollowerRelationshipUserRepository: IGenericRepository<FollowerRelationshipUserEntity>
{
    Task<bool> ExistsByFollowerIdAndFollowedId(string followerId, string followedId);
    Task<FollowerRelationshipUserEntity?> GetByFollowerIdAndFollowedId(string followerId, string followedId);
}