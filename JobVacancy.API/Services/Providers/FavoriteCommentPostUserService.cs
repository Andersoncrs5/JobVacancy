using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class FavoriteCommentPostUserService(IUnitOfWork uow): IFavoriteCommentPostUserService
{
    public async Task<IQueryable<FavoriteCommentEntity>> GetAllQuery()
    {
        return uow.FavoriteCommentPostUserRepository.GetAllQuery();
    }

    public async Task<FavoriteCommentEntity?> GetByCommentIdAndUserId(string commentId, string userId)
    {
        return await uow.FavoriteCommentPostUserRepository.GetByCommentIdAndUserId(commentId, userId);
    }

    public async Task Delete(FavoriteCommentEntity favoriteComment)
    {
        uow.FavoriteCommentPostUserRepository.Delete(favoriteComment);
        await uow.Commit();
    }
    
    public async Task<bool> ExistsByCommentIdAndUserId(string commentId, string userId)
    {
        return await uow.FavoriteCommentPostUserRepository.ExistsByCommentIdAndUserId(commentId, userId);
    }

    public async Task Create(string commentId, string userId)
    {
        FavoriteCommentEntity favoriteComment = new FavoriteCommentEntity
        {
            CommentId = commentId,
            UserId = userId,
        };
        
        await uow.FavoriteCommentPostUserRepository.AddAsync(favoriteComment);
        await uow.Commit();
    }
    
}