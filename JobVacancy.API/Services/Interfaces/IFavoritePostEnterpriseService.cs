using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IFavoritePostEnterpriseService
{
    Task<FavoritePostEnterpriseEntity?> GetByUserIdAndPostIdWithEntity(string userId, string postId);
    Task<FavoritePostEnterpriseEntity?> GetByUserIdAndPostId(string userId, string postId);
    Task<bool> ExistsByUserIdAndPostId(string userId, string postId);
    Task Delete(FavoritePostEnterpriseEntity favor);
    IQueryable<FavoritePostEnterpriseEntity> Query();
    Task<FavoritePostEnterpriseEntity> Create(string userId, string postId);
}