using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters;

public class FilterBaseQuery
{
    public static IQueryable<TEntity> ApplyBaseFilters<TEntity, TFilter>(
        IQueryable<TEntity> query, 
        TFilter filterParams)
        where TEntity : BaseEntity 
        where TFilter : FilterBaseParams 
    {
        
        if (!string.IsNullOrWhiteSpace(filterParams.Id))
        {
            query = query.Where(e => EF.Functions.Like(e.Id, $"%{filterParams.Id}%"));
        }
        
        if (filterParams.CreatedAfter.HasValue)
        {
            var createdAfterUtc = filterParams.CreatedAfter.Value.ToUniversalTime();
            query = query.Where(e => e.CreatedAt.ToUniversalTime() >= createdAfterUtc);
        }
        
        if (filterParams.CreatedBefore.HasValue)
        {
            var createdBeforeUtc = filterParams.CreatedBefore.Value.ToUniversalTime();
            query = query.Where(e => e.CreatedAt.ToUniversalTime() <= createdBeforeUtc);
        }

        return query;
    }
}