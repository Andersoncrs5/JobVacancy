using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class FavoriteCommentPostEnterpriseService(IUnitOfWork uow): IFavoriteCommentPostEnterpriseService
{
    public IQueryable<FavoriteCommentEntity> GetAllQuery()
    {
        return uow.FavoriteCommentPostUserRepository.GetAllQuery();
    }
    
    public async Task<FavoriteCommentEntity?> GetByCommentIdAndUserId(string commentId, string userId)
    {
        return await uow.FavoriteCommentPostEnterpriseRepository.GetByCommentIdAndUserId(commentId, userId);
    }
    
    public async Task Delete(FavoriteCommentEntity favoriteComment)
    {
        uow.FavoriteCommentPostEnterpriseRepository.Delete(favoriteComment);
        await uow.Commit();
    }
    
    public async Task<bool> ExistsByCommentIdAndUserId(string commentId, string userId)
    {
        return await uow.FavoriteCommentPostEnterpriseRepository.ExistsByCommentIdAndUserId(commentId, userId);
    }

    public async Task Create(string commentId, string userId)
    {
        FavoriteCommentEntity favoriteComment = new FavoriteCommentEntity
        {
            CommentId = commentId,
            UserId = userId
        };
        
        await uow.FavoriteCommentPostEnterpriseRepository.AddAsync(favoriteComment);
        await uow.Commit();
    }
    
}