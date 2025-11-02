using JobVacancy.API.models.dtos.FavoritePost;
using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IFavoritePostUserService
{
    Task<FavoritePostUserEntity?> GetByUserIdAndPostUserIdWithEntity(string userId, string postUserId);
    Task<FavoritePostUserEntity?> GetByUserIdAndPostUserId(string userId, string postUserId);
    Task<bool> ExistsByUserIdAndPostUserId(string userId, string postUserId);
    Task Delete(FavoritePostUserEntity favoritePostUser);
    Task<FavoritePostUserEntity> Update(FavoritePostUserEntity favoritePostUser, UpdateFavoritePostDto dto);
    IQueryable<FavoritePostUserEntity> Query();
    Task<FavoritePostUserEntity> Create(string userId, string postUserId);
}