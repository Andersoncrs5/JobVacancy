using JobVacancy.API.models.entities;
using JobVacancy.API.Utils.Filters.BaseQuery;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.CommentPostEnterprise;

public class CommentPostEnterpriseFilterQuery
{
    public static IQueryable<CommentPostEnterpriseEntity> ApplyFilter(IQueryable<CommentPostEnterpriseEntity> query,
        CommentPostEnterpriseFilterParam filter)
    {
        query = query.Include(x => x.Post);
        query = query.Include(x => x.User);
        
        query = CommentBaseFilterQuery.ApplyBaseFilters(query, filter);
        
        if (!string.IsNullOrEmpty(filter.CategoryId))
        {
            query = query.Where(e => e.Post!.CategoryId  == filter.CategoryId);
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

        if (filter.Type != null || !string.IsNullOrWhiteSpace(filter.NameEnterprise) || filter.Type.HasValue)
        {
            query = query.Include(x =>x.Post!.Enterprise);
        }

        if (!string.IsNullOrEmpty(filter.EnterpriseId))
        {
            query = query.Where(x => x.Post!.EnterpriseId == filter.EnterpriseId);
        }
        
        if (!string.IsNullOrEmpty(filter.NameEnterprise))
        {
            query = query.Where(x => x.Post!.Enterprise!.Name == filter.NameEnterprise);
        }
        
        if (filter.Type.HasValue)
        {
            query = query.Where(x => x.Post!.Enterprise!.Type == filter.Type);
        }
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);

    }
}