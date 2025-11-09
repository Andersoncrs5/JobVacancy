using JobVacancy.API.models.entities;
using JobVacancy.API.Utils.Filters.BaseQuery;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.CommentPostUser;

public class CommentPostUserFilterQuery
{
    public static IQueryable<CommentPostUserEntity> ApplyFilter(IQueryable<CommentPostUserEntity> query, CommentPostUserFilterParam filter)
    {
        query = query.Include(x => x.User);
        
        if (
            !string.IsNullOrWhiteSpace(filter.PostId) ||
            !string.IsNullOrWhiteSpace(filter.Title) ||
            !string.IsNullOrWhiteSpace(filter.ContentPost) ||
            filter.ReadingTimeMinutesMin.HasValue ||
            filter.ReadingTimeMinutesMax.HasValue
        )
        {
            query = query.Include(x => x.Post);
        }
        
        query = CommentBaseFilterQuery.ApplyBaseFilters(query, filter);

        if (!string.IsNullOrEmpty(filter.CategoryId))
        {
            query = query.Where(e => e.Post!.CategoryId  == filter.CategoryId);
        }
        
        if (!string.IsNullOrEmpty(filter.PostId))
        {
            query = query.Where(e => e.Post!.Id  == filter.PostId);
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            query = query.Where(e => EF.Functions.Like(e.Post!.Title, $"%{filter.Title}%"));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.ContentPost))
        {
            query = query.Where(e => EF.Functions.Like(e.Post!.Content, $"%{filter.ContentPost}%"));
        }
        
        if (filter.ReadingTimeMinutesMin.HasValue)
        {
            query = query.Where(e => e.Post!.ReadingTimeMinutes >= filter.ReadingTimeMinutesMin.Value);
        }
        
        if (filter.ReadingTimeMinutesMax.HasValue)
        {
            query = query.Where(e => e.Post!.ReadingTimeMinutes <= filter.ReadingTimeMinutesMax.Value);
        }
        
        return FilterBaseQuery.ApplyBaseFilters<CommentPostUserEntity, CommentPostUserFilterParam>(query, filter);
    }
}