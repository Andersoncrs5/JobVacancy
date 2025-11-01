using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.Skill;

public class SkillFilterQuery
{
    public static IQueryable<SkillEntity> ApplyFilter(IQueryable<SkillEntity> query, SkillFilterParam filter)
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