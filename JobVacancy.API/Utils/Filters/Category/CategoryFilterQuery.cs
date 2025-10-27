using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.Category;

public class CategoryFilterQuery
{
    public static IQueryable<CategoryEntity> ApplyFilter(IQueryable<CategoryEntity> query, CategoryFilterParams filter)
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
        
        return FilterBaseQuery.ApplyBaseFilters<CategoryEntity, CategoryFilterParams>(query, filter);
    }
}