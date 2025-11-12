using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.Area;

public class AreaFilterQuery
{
    public static IQueryable<AreaEntity> ApplyFilter(IQueryable<AreaEntity> query, AreaFilterParams filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(c => EF.Functions.Like(c.Name, $"%{filter.Name}%"));
            
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Description))
        {
            query = query.Where(c => EF.Functions.Like(c.Description, $"%{filter.Description}%"));
        }
        
        if (filter.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == filter.IsActive);
        }
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}