using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.Enterprise;

public class EnterpriseFilterQuery
{
    public static IQueryable<EnterpriseEntity> ApplyFilter(IQueryable<EnterpriseEntity> query,
        EnterpriseFilterParam filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(c => EF.Functions.Like(c.Name, $"%{filter.Name}%"));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Description))
        {
            query = query.Where(c => EF.Functions.Like(c.Description, $"%{filter.Description}%"));
        }

        if (filter.Type.HasValue)
        {
            query = query.Where(c => c.Type == filter.Type);
        }

        if (filter.UserFilterParams != null)
        {
            query = query.Include(u => u.User);
        }
        
        if (filter.UserFilterParams != null && !string.IsNullOrEmpty(filter.UserFilterParams.Email))
        {
            query = query.Where(c => c.User.Email == filter.UserFilterParams.Email);
        }
        
        if (filter.UserFilterParams != null && !string.IsNullOrEmpty(filter.UserFilterParams.UserName))
        {
            query = query.Where(c => c.User.UserName == filter.UserFilterParams.UserName);
        }
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}