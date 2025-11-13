using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.VacancySkill;

public class VacancySkillFilterQuery
{
    public static IQueryable<VacancySkillEntity> ApplyFilter(IQueryable<VacancySkillEntity> query, VacancySkillFilterParams filter)
    {
        query = query.Include(x => x.Skill).Include(x => x.Vacancy);
        
        if (!string.IsNullOrWhiteSpace(filter.VacancyTitle))
            query = query.Where(c => EF.Functions.Like(c.Vacancy!.Title, $"%{filter.VacancyTitle}%"));

        if (!string.IsNullOrWhiteSpace(filter.VacancyId))
            query = query.Where(c => c.VacancyId == filter.VacancyId);
        
        if (!string.IsNullOrWhiteSpace(filter.SkillId))
            query = query.Where(c => c.SkillId == filter.SkillId);
        
        if (!string.IsNullOrWhiteSpace(filter.SkillName))
            query = query.Where(c => c.Skill!.Name == filter.SkillName);
        
        if (filter.RequiredLevel.HasValue)
            query = query.Where(c => c.RequiredLevel == filter.RequiredLevel.Value);
        
        if (filter.IsMandatory.HasValue)
            query = query.Where(c => c.IsMandatory == filter.IsMandatory.Value);
        
        query = query
            .Where(x => !filter.WeightMin.HasValue || x.Weight >= filter.WeightMin.Value)
            .Where(x => !filter.WeightMax.HasValue || x.Weight <= filter.WeightMax.Value);

        query = query
            .Where(x => !filter.YearsOfExperienceRequiredMin.HasValue || x.YearsOfExperienceRequired >= filter.YearsOfExperienceRequiredMin.Value)
            .Where(x => !filter.YearsOfExperienceRequiredMax.HasValue || x.YearsOfExperienceRequired <= filter.YearsOfExperienceRequiredMax.Value);
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}