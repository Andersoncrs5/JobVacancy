using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace JobVacancy.API.Repositories.Provider;

public class FavoriteCommentPostUserRepository(AppDbContext db): GenericRepository<FavoriteCommentEntity>(db),  IFavoriteCommentPostUserRepository
{
    public IQueryable<FavoriteCommentEntity> GetAllQuery() 
    {
        var baseQuery = db.FavoriteCommentEntities
            .AsQueryable();

        var queryWithComment = baseQuery.Include(f => f.Comment);
        
         IQueryable<FavoriteCommentEntity> finalQuery = 
            queryWithComment.Where(f => f.Comment != null)
               .Where(f => f.Comment is CommentPostUserEntity);
        
        return finalQuery.AsSplitQuery(); 
    }

    public async Task<FavoriteCommentEntity?> GetByCommentIdAndUserId(string commentId, string userId)
    {
        return await db.FavoriteCommentEntities
            .Where(f => f.Comment != null && f.Comment is CommentPostUserEntity)
            .FirstOrDefaultAsync(f => f.CommentId == commentId && f.UserId == userId);
    }
    
    
    
}