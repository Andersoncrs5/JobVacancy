using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;
namespace JobVacancy.API.Utils.Filters.IndicationUser;

public class IndicationUserFilterQuery
{
    public static IQueryable<IndicationUserEntity> ApplyFilter(IQueryable<IndicationUserEntity> query,
        IndicationUserFilterParam filter)
    {
        query = query.Include(x => x.Endorsed);
        query = query.Include(e => e.Endorser);
        
        if (!string.IsNullOrWhiteSpace(filter.EndorserId))
            query = query.Where(x => x.EndorserId == filter.EndorserId);
        
        if (!string.IsNullOrWhiteSpace(filter.EndorserName))
            query = query.Where(x => x.Endorser!.UserName!.Contains(filter.EndorserName));
        
        if (!string.IsNullOrWhiteSpace(filter.EndorserEmail))
            query = query.Where(x => x.Endorser!.Email!.Contains(filter.EndorserEmail));
        
        if (!string.IsNullOrWhiteSpace(filter.EndorsedId))
            query = query.Where(x => x.EndorsedId == filter.EndorsedId);
        
        if (!string.IsNullOrWhiteSpace(filter.EndorsedName))
            query = query.Include(x => x.Endorsed!.UserName!.Contains(filter.EndorsedName));
        
        if (!string.IsNullOrWhiteSpace(filter.EndorsedEmail))
            query = query.Include(x => x.Endorsed!.Email!.Contains(filter.EndorsedEmail));
        
        if (!string.IsNullOrWhiteSpace(filter.Content))
            query = query.Where(x => x.Content.Contains(filter.Content));
        
        if (filter.Status.HasValue)
            query = query.Where(x => x.Status == filter.Status.Value);
        
        query = query
            .Where(x => !filter.SkillRatingMin.HasValue || x.SkillRating >= filter.SkillRatingMin.Value)
            .Where(x => !filter.SkillRatingMax.HasValue || x.SkillRating <= filter.SkillRatingMax.Value);
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}