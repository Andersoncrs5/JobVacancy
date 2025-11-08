using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.Position;

public class PositionFilterQuery
{
    public static IQueryable<PositionEntity> ApplyFilter(IQueryable<PositionEntity> query,
        PositionFilterParams filter)
    {
        if (filter.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == filter.IsActive);
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(c => EF.Functions.Like(c.Name, $"%{filter.Name}%"));
        }
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}