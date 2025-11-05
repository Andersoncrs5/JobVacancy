using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Repositories.Provider;

public class FavoriteCommentPostEnterpriseRepository(AppDbContext db): GenericRepository<FavoriteCommentEntity>(db),  IFavoriteCommentPostEnterpriseRepository
{
    public IQueryable<FavoriteCommentEntity> GetAllQuery()
    {
        IQueryable<FavoriteCommentEntity> queryable = db.FavoriteCommentEntities.AsQueryable();
        
        var queryWithComment = queryable.Include(x => x.Comment);

        IQueryable<FavoriteCommentEntity> final = queryWithComment.Where(x => x.Comment != null).Where(x => x.Comment is CommentPostEnterpriseEntity);
        return final.AsSplitQuery();
    }
    
    public async Task<FavoriteCommentEntity?> GetByCommentIdAndUserId(string commentId, string userId)
    {
        return await db.FavoriteCommentEntities
            .Where(f => f.Comment != null && f.Comment is CommentPostEnterpriseEntity)
            .FirstOrDefaultAsync(f => f.CommentId == commentId && f.UserId == userId);
    }
    
    public async Task<bool> ExistsByCommentIdAndUserId(string commentId, string userId)
    {
        return await db.FavoriteCommentEntities
            .Where(f => f.Comment != null && f.Comment is CommentPostEnterpriseEntity)
            .AnyAsync(f => f.CommentId == commentId && f.UserId == userId);
    }
    
}