using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IFavoriteCommentPostUserService
{
    Task<IQueryable<FavoriteCommentEntity>> GetAllQuery();
    Task<FavoriteCommentEntity?> GetByCommentIdAndUserId(string commentId, string userId);
    Task Delete(FavoriteCommentEntity favoriteComment);
    Task<bool> ExistsByCommentIdAndUserId(string commentId, string userId);
    Task Create(string commentId, string userId);
}