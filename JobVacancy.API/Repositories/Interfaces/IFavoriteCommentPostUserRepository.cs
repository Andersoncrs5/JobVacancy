using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IFavoriteCommentPostUserRepository: IGenericRepository<FavoriteCommentEntity>
{
    IQueryable<FavoriteCommentEntity> GetAllQuery();
    Task<FavoriteCommentEntity?> GetByCommentIdAndUserId(string commentId, string userId);
    Task<bool> ExistsByCommentIdAndUserId(string commentId, string userId);
}