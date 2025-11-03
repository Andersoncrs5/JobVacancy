using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IFavoritePostEnterpriseRepository: IGenericRepository<FavoritePostEnterpriseEntity>
{
    Task<FavoritePostEnterpriseEntity?> GetByUserIdAndPostId(string userId, string postUserId);
    Task<bool> ExistsByUserIdAndPostId(string userId, string postId);
    Task<FavoritePostEnterpriseEntity?> GetByUserIdAndPostIdWithEntity(string userId, string postId);
}