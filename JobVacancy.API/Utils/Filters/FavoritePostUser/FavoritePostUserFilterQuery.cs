using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.FavoritePostUser;

public class FavoritePostUserFilterQuery
{
    public static IQueryable<FavoritePostUserEntity> ApplyFilter(IQueryable<FavoritePostUserEntity> query, FavoritePostUserFilterParam filter)
    {
        query = query.Include(e => e.PostUser);
        query = query.Include(e => e.User);
        
        if (!string.IsNullOrWhiteSpace(filter.UserId))
        {
            query = query.Where(c => c.UserId == filter.UserId);
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
            query = query.Where(c => c.PostUser!.ReadingTimeMinutes >= filter.ReadingTimeMinutesMin);
        }

        if (filter.ReadingTimeMinutesMax.HasValue)
        {
            query = query.Where(c => c.PostUser!.ReadingTimeMinutes <= filter.ReadingTimeMinutesMax);
        }
        
        if (!string.IsNullOrWhiteSpace(filter.PostUserId))
        {
            query = query.Where(c => c.PostUserId == filter.PostUserId);
        }

        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            query = query.Where(c => c.PostUser!.Title!.Contains(filter.Title));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.CategoryId))
        {
            query = query.Where(c => c.PostUser!.CategoryId!.Contains(filter.CategoryId));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.UserNotes))
        {
            query = query.Where(c => c.UserNotes!.Contains(filter.UserNotes));
        }
        
        if (filter.UserRatingMin.HasValue)
        {
            query = query.Where(c => c.UserRating >= filter.UserRatingMin);
        }

        if (filter.UserRatingMax.HasValue)
        {
            query = query.Where(c => c.UserRating <= filter.UserRatingMax);
        }
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}