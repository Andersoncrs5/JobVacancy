using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.PostUser;

public class PostEnterpriseFilterQuery
{
    public static IQueryable<PostEnterpriseEntity> ApplyFilter(IQueryable<PostEnterpriseEntity> query, PostEnterpriseFilterParam filter)
    {
        query = query.Include(c => c.Enterprise);

        if (
            !string.IsNullOrWhiteSpace(filter.CategoryId) || 
            !string.IsNullOrWhiteSpace(filter.NameCategory)
        )
        {
            query = query.Include(c => c.Category);
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            query = query.Where(c => EF.Functions.Like(c.Title, $"%{filter.Title}%"));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Content))
        {
            query = query.Where(c => c.Content.Contains(filter.Content));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == filter.IsActive);
        }

        if (filter.IsFeatured.HasValue)
        {
            query = query.Where(c => c.IsFeatured == filter.IsFeatured);
        }

        if (filter.ReadingTimeMinutesBefore.HasValue)
        {
            query = query.Where(c => c.ReadingTimeMinutes <= filter.ReadingTimeMinutesBefore.Value);
        }

        if (filter.ReadingTimeMinutesAfter.HasValue)
        {
            query = query.Where(c => c.ReadingTimeMinutes >= filter.ReadingTimeMinutesAfter.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.CategoryId))
        {
            query = query.Where(c => c.CategoryId == filter.CategoryId);
        }

        if (!string.IsNullOrWhiteSpace(filter.NameCategory))
        {
            query = query.Where(c => c.Category!.Name.Contains(filter.NameCategory));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.NameEnterprise))
        {
            query = query.Where(c => c.Enterprise!.Name.Contains(filter.NameEnterprise));
        }
        
        if (filter.TypeEnterprise.HasValue)
        {
            query = query.Where(c => c.Enterprise!.Type == filter.TypeEnterprise);
        }
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}