using JobVacancy.API.models.entities;
using JobVacancy.API.models.entities.Base;
using JobVacancy.API.Utils.Filters.Base;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.BaseQuery;

public class CommentBaseFilterQuery
{
    public static IQueryable<TEntity> ApplyBaseFilters<TEntity, TFilter>(
        IQueryable<TEntity> query,
        TFilter filterParams ) 
        where TEntity : CommentBaseEntity
        where TFilter : CommentBaseFilterParams
    {
        
        if (!string.IsNullOrWhiteSpace(filterParams.Content))
        {
            query = query.Where(e => EF.Functions.Like(e.Content, $"%{filterParams.Content}%"));
        }

        if (filterParams.IsActive.HasValue)
        {
            query = query.Where(e => e.IsActive  == filterParams.IsActive.Value);
        }
        
        if (filterParams.DepthMin.HasValue)
        {
            query = query.Where(e => e.Depth >= filterParams.DepthMin.Value);
        }
        
        if (filterParams.DepthMax.HasValue)
        {
            query = query.Where(e => e.Depth <= filterParams.DepthMax.Value);
        }

        if (!string.IsNullOrEmpty(filterParams.ParentCommentId))
        {
            query = query.Where(e => e.ParentCommentId == filterParams.ParentCommentId);
        }
     
        if (string.IsNullOrEmpty(filterParams.ParentCommentId))
        {
            query = query.Where(e => e.ParentCommentId == null);
        }
        
        if (!string.IsNullOrEmpty(filterParams.UserId))
        {
            query = query.Where(e => e.UserId  == filterParams.UserId);
        }
        
        if (!string.IsNullOrEmpty(filterParams.Username))
        {
            query = query.Where(e => e.User!.UserName  == filterParams.Username);
        }
        
        if (!string.IsNullOrEmpty(filterParams.Email))
        {
            query = query.Where(e => e.User!.Email  == filterParams.Email);
        }
          
        if (!string.IsNullOrEmpty(filterParams.Fullname))
        {
            query = query.Where(e => e.User!.FullName  == filterParams.Fullname);
        }
        
        return query;
    }

}