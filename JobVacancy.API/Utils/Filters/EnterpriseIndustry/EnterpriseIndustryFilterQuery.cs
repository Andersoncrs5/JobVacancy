using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.EnterpriseIndustry;

public class EnterpriseIndustryFilterQuery
{
    public static IQueryable<EnterpriseIndustryEntity> ApplyFilters(IQueryable<EnterpriseIndustryEntity> query,
        EnterpriseIndustryFilterParams filter)
    {
        query = query.Include(x => x.Enterprise);
        query = query.Include(x => x.Industry);
        
        if (filter.EnterpriseFilterParam != null)
        {
            query = query.Include(c =>c.Enterprise);
        }

        if (filter.EnterpriseFilterParam != null && filter.EnterpriseFilterParam.UserFilterParams != null)
        {
            query = query.Include(c =>c.Enterprise.User);
        }

        if (!string.IsNullOrWhiteSpace(filter.EnterpriseId))
        {
            query = query.Where(c => c.EnterpriseId == filter.EnterpriseId );
        }
        
        if (!string.IsNullOrWhiteSpace(filter.IndustryId))
        {
            query = query.Where(c => c.IndustryId == filter.IndustryId );
        }
        
        if (filter.EnterpriseFilterParam != null && !string.IsNullOrEmpty(filter.EnterpriseFilterParam.Name))
        {
            query = query.Where(c => EF.Functions.Like(c.Enterprise.Name, $"%{filter.EnterpriseFilterParam.Name}%"));
        }
        
        if (filter.EnterpriseFilterParam != null && filter.EnterpriseFilterParam.Type != null)
        {
            query = query.Where(c => c.Enterprise.Type == filter.EnterpriseFilterParam.Type);
        }
        
        if (filter.EnterpriseFilterParam != null && filter.EnterpriseFilterParam.Id != null)
        {
            query = query.Where(c => c.Enterprise.Id == filter.EnterpriseFilterParam.Id);
        }
        
        
        
        
        if (filter.IndustryFilterParams != null) 
        {
            query = query.Include(c =>c.Industry);
        }

        if (filter.IndustryFilterParams != null && !string.IsNullOrEmpty(filter.IndustryFilterParams.Name))
        {
            query  = query.Where(c => EF.Functions.Like(c.Industry.Name, $"%{filter.IndustryFilterParams.Name}%"));
        }
        
        if (filter.IndustryFilterParams != null && !string.IsNullOrEmpty(filter.IndustryFilterParams.Id))
        {
            query  = query.Where(c => c.Industry.Id == filter.IndustryFilterParams.Id);
        }
        
        if (filter.IndustryFilterParams != null && filter.IndustryFilterParams.IsActive.HasValue)
        {
            query  = query.Where(c => c.Industry.IsActive == filter.IndustryFilterParams.IsActive);
        }
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}