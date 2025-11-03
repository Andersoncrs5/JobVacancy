using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.FavoritePostEnterprise;

public class FavoritePostEnterpriseFilterQuery
{
    public static IQueryable<FavoritePostEnterpriseEntity> ApplyFilter(IQueryable<FavoritePostEnterpriseEntity> query,
        FavoritePostEnterpriseFilterParam filter)
    {
        query = query.Include(e => e.PostEnterprise);
        query = query.Include(e => e.User);
        
        if (!string.IsNullOrWhiteSpace(filter.UserId))
        {
            query = query.Where(x => x.UserId == filter.UserId);
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Fullname))
        {
            query = query.Where(c => c.User!.FullName!.Contains(filter.Fullname));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Username))
        {
            query = query.Where(c => c.User!.UserName!.Contains(filter.Username));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Email))
        {
            query = query.Where(c => c.User!.Email!.Contains(filter.Email));
        }
        
        if (filter.ReadingTimeMinutesMin.HasValue)
        {
            query = query.Where(c => c.PostEnterprise!.ReadingTimeMinutes >= filter.ReadingTimeMinutesMin);
        }

        if (filter.ReadingTimeMinutesMax.HasValue)
        {
            query = query.Where(c => c.PostEnterprise!.ReadingTimeMinutes <= filter.ReadingTimeMinutesMax);
        }
        
        if (!string.IsNullOrWhiteSpace(filter.PostId))
        {
            query = query.Where(c => c.PostEnterpriseId == filter.PostId);
        }

        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            query = query.Where(c => c.PostEnterprise!.Title!.Contains(filter.Title));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.CategoryId))
        {
            query = query.Where(c => c.PostEnterprise!.CategoryId!.Contains(filter.CategoryId));
        }
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}