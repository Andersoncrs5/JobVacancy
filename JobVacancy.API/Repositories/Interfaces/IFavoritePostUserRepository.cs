using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IFavoritePostUserRepository: IGenericRepository<FavoritePostUserEntity>
{
    Task<FavoritePostUserEntity?> GetByUserIdAndPostUserId(string userId, string postUserId);
    Task<bool> ExistsByUserIdAndPostUserId(string userId, string postUserId);
    Task<FavoritePostUserEntity?> GetByUserIdAndPostUserIdWithEntity(string userId, string postUserId);
}