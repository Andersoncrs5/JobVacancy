using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.PostUser;

public class PostUserFilterQuery
{
    public static IQueryable<PostUserEntity> ApplyFilter(IQueryable<PostUserEntity> query, PostUserFilterParam filter)
    {
        query = query.Include(c => c.User);

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
        
        if (!string.IsNullOrEmpty(filter.UserId))
        {
            query = query.Where(c => c.UserId == filter.UserId);
        }
        
        if (!string.IsNullOrEmpty(filter.FullNameUser))
        {
            query = query.Where(c => c.User!.FullName.Contains(filter.FullNameUser));
        }
        
        if (!string.IsNullOrEmpty(filter.UserName))
        {
            query = query.Where(c => c.User!.UserName!.Contains(filter.UserName));
        }
        
        if (!string.IsNullOrEmpty(filter.EmailUser))
        {
            query = query.Where(c => c.User!.Email!.Contains(filter.EmailUser));
        }
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}