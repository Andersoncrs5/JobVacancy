using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.Utils.Filters.Vacancy;

public class VacancyFilterParam: FilterBaseParams
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Requirements { get; set; }
    public string? Responsibilities { get; set; }
    public string? Benefits { get; set; }
    
    public EmploymentTypeEnum? EmploymentType { get; set; }
    public ExperienceLevelEnum? ExperienceLevel { get; set; }
    public EducationLevelEnum? EducationLevel { get; set; }
    public WorkplaceTypeEnum? WorkplaceType { get; set; }
    public CurrencyEnum? Currency { get; set; }
    public VacancyStatusEnum? Status { get; set; }
    
    public int? SeniorityMin { get; set; }
    public int? SeniorityMax { get; set; }
 
    public int? OpeningMin { get; set; }
    public int? OpeningMax { get; set; }
    
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }

    public string? EnterpriseId { get; set; }
    public string? EnterpriseName { get; set; }
    public EnterpriseTypeEnum? EnterpriseType { get; set; }

    public string? AreaId { get; set; }
    public string? AreaName { get; set; }
    
    public DateTime? ApplicationDeadLineMin { get; set; }
    public DateTime? ApplicationDeadLineMax { get; set; }
}