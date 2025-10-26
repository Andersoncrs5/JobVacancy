using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.User;

public static class UserFilterQuery
{
    public static IQueryable<UserEntity> ApplyFilter(IQueryable<UserEntity> query, UserFilterParams filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.UserName))
        {
            query = query.Where(c => EF.Functions.Like(c.UserName, $"%{filter.UserName}%"));
            
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Id))
        {
            query = query.Where(c => EF.Functions.Like(c.Id, $"%{filter.Id}%"));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.FullName))
        {
            query = query.Where(c => EF.Functions.Like(c.FullName, $"%{filter.FullName}%"));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Email))
        {
            query = query.Where(c => EF.Functions.Like(c.Email, $"%{filter.Email}%"));
        }

        if (filter.CreatedAfter.HasValue)
        {
            query = query.Where(c => c.CreatedAt.ToUniversalTime() >= filter.CreatedAfter.Value.ToUniversalTime());
        }

        if (filter.CreatedBefore.HasValue)
        {
            query = query.Where(c => c.CreatedAt.ToUniversalTime() <= filter.CreatedBefore.Value.ToUniversalTime());
        }

        return query;

    }
}