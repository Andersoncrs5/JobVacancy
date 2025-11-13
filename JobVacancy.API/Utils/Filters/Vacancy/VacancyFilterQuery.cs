using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.Vacancy;

public class VacancyFilterQuery
{
    public static IQueryable<VacancyEntity> ApplyFilter(IQueryable<VacancyEntity> query, VacancyFilterParam filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.EnterpriseName) || filter.EnterpriseType.HasValue)
            query = query.Include(x => x.Enterprise);
        
        if (!string.IsNullOrWhiteSpace(filter.AreaName))
            query = query.Include(x => x.Area);
        
        if (!string.IsNullOrWhiteSpace(filter.Title))
            query = query.Where(c => EF.Functions.Like(c.Title, $"%{filter.Title}%"));
        
        if (!string.IsNullOrWhiteSpace(filter.Description))
            query = query.Where(c => EF.Functions.Like(c.Description, $"%{filter.Description}%"));
        
        if (!string.IsNullOrWhiteSpace(filter.Requirements))
            query = query.Where(c => EF.Functions.Like(c.Requirements, $"%{filter.Requirements}%"));
        
        if (!string.IsNullOrWhiteSpace(filter.Responsibilities))
            query = query.Where(c => EF.Functions.Like(c.Responsibilities, $"%{filter.Responsibilities}%"));
        
        if (!string.IsNullOrWhiteSpace(filter.Benefits))
            query = query.Where(c => EF.Functions.Like(c.Benefits, $"%{filter.Benefits}%"));
        
        if (filter.EmploymentType.HasValue)
            query = query.Where(c => c.EmploymentType == filter.EmploymentType.Value);
        
        if (filter.ExperienceLevel.HasValue)
            query = query.Where(c => c.ExperienceLevel == filter.ExperienceLevel.Value);
        
        if (filter.EducationLevel.HasValue)
            query = query.Where(c => c.EducationLevel == filter.EducationLevel.Value);
        
        if (filter.WorkplaceType.HasValue)
            query = query.Where(c => c.WorkplaceType == filter.WorkplaceType.Value);
        
        if (filter.Currency.HasValue)
            query = query.Where(c => c.Currency == filter.Currency.Value);
        
        if (filter.Status.HasValue)
            query = query.Where(c => c.Status == filter.Status.Value);
        
        query = query
            .Where(x => !filter.OpeningMin.HasValue || x.Opening >= filter.OpeningMin.Value)
            .Where(x => !filter.OpeningMax.HasValue || x.Opening <= filter.OpeningMax.Value);
        
        query = query
            .Where(x => !filter.SalaryMin.HasValue || x.SalaryMin >= filter.SalaryMin.Value)
            .Where(x => !filter.SalaryMax.HasValue || x.SalaryMax <= filter.SalaryMax.Value);
        
        query = query
            .Where(x => !filter.SeniorityMin.HasValue || x.Seniority >= filter.SeniorityMin.Value)
            .Where(x => !filter.SeniorityMax.HasValue || x.Seniority <= filter.SeniorityMax.Value);
        
        query = query
            .Where(x => !filter.ApplicationDeadLineMin.HasValue || x.ApplicationDeadLine >= filter.ApplicationDeadLineMin.Value)
            .Where(x => !filter.ApplicationDeadLineMax.HasValue || x.ApplicationDeadLine <= filter.ApplicationDeadLineMax.Value);
        
        // FILTER ENTERPRISE
        if (!string.IsNullOrWhiteSpace(filter.EnterpriseName))
            query = query.Where(c => EF.Functions.Like(c.Enterprise!.Name, $"%{filter.EnterpriseName}%"));

        if (!string.IsNullOrWhiteSpace(filter.EnterpriseId))
            query = query.Where(c => EF.Functions.Like(c.EnterpriseId, $"%{filter.EnterpriseId}%"));

        if (filter.EnterpriseType.HasValue)
            query = query.Where(x => x.Enterprise!.Type == filter.EnterpriseType.Value);
        
        // FILTER BY AREA
        if (!string.IsNullOrWhiteSpace(filter.AreaId))
            query = query.Where(c => EF.Functions.Like(c.AreaId, $"%{filter.AreaId}%"));

        if (!string.IsNullOrWhiteSpace(filter.AreaName))
            query = query.Where(c => EF.Functions.Like(c.Area!.Name, $"%{filter.AreaName}%"));
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}