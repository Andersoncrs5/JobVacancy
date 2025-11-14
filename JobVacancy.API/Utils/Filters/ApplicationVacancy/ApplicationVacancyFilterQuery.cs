using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.ApplicationVacancy;

public class ApplicationVacancyFilterQuery
{
    public static IQueryable<ApplicationVacancyEntity> ApplyFilter(IQueryable<ApplicationVacancyEntity> query, ApplicationVacancyFilterParams filter)
    {
        if (
            filter.LoadVacancy.HasValue ||
            !string.IsNullOrWhiteSpace(filter.VacancyTitle) ||
            filter.EmploymentType.HasValue ||
            filter.ExperienceLevel.HasValue ||
            filter.EducationLevel.HasValue ||
            filter.WorkplaceType.HasValue ||
            filter.Currency.HasValue
        ) 
        {
            query = query.Include(x => x.Vacancy);
        }
        
        if (
            filter.LoadUser.HasValue ||
            !string.IsNullOrWhiteSpace(filter.Username) || 
            !string.IsNullOrWhiteSpace(filter.Email)  
            )
            query = query.Include(x => x.User);
        
        // VACANCY
        if (!string.IsNullOrWhiteSpace(filter.VacancyId))
            query = query.Where(x => x.VacancyId == filter.VacancyId);
        
        if (!string.IsNullOrWhiteSpace(filter.VacancyTitle))
            query = query.Where(x => EF.Functions.Like(x.Vacancy!.Title, $"%{filter.VacancyTitle}%"));
        
        if (filter.EmploymentType.HasValue)
            query = query.Where(x => x.Vacancy!.EmploymentType == filter.EmploymentType.Value);
        
        if (filter.ExperienceLevel.HasValue)
            query = query.Where(x => x.Vacancy!.ExperienceLevel == filter.ExperienceLevel.Value);
        
        if (filter.EducationLevel.HasValue)
            query = query.Where(x => x.Vacancy!.EducationLevel == filter.EducationLevel.Value);
        
        if (filter.WorkplaceType.HasValue)
            query = query.Where(x => x.Vacancy!.WorkplaceType == filter.WorkplaceType.Value);
        
        if (filter.Currency.HasValue)
            query = query.Where(x => x.Vacancy!.Currency == filter.Currency.Value);
        
        // USER
        
        if (!string.IsNullOrWhiteSpace(filter.UserId))
            query = query.Where(x => x.UserId == filter.UserId);
        
        if (!string.IsNullOrWhiteSpace(filter.Username))
            query = query.Where(x => EF.Functions.Like(x.User!.UserName, $"%{filter.Username}%"));
        
        if (!string.IsNullOrWhiteSpace(filter.Email))
            query = query.Where(x => EF.Functions.Like(x.User!.Email, $"%{filter.Email}%"));
        
        // APPLICATION
        if (filter.Status.HasValue)
            query = query.Where(x => x.Status == filter.Status.Value);
        
        if (filter.IsViewedByRecruiter.HasValue)
            query = query.Where(x => x.IsViewedByRecruiter == filter.IsViewedByRecruiter.Value);
        
        query = query
            .Where(x => !filter.ScoreMin.HasValue || x.Score >= filter.ScoreMin.Value)
            .Where(x => !filter.ScoreMax.HasValue || x.Score <= filter.ScoreMax.Value);
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}