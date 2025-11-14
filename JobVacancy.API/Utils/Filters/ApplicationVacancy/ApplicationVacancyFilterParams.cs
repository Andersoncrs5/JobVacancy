using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.Utils.Filters.ApplicationVacancy;

public class ApplicationVacancyFilterParams: FilterBaseParams
{
    public string? VacancyId { get; set; }
    public string? VacancyTitle{ get; set; }
    public EmploymentTypeEnum? EmploymentType { get; set; }
    public ExperienceLevelEnum? ExperienceLevel { get; set; }
    public EducationLevelEnum? EducationLevel { get; set; }
    public WorkplaceTypeEnum? WorkplaceType { get; set; }
    public CurrencyEnum? Currency { get; set; }
    
    public string? UserId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    
    public ApplicationStatusEnum? Status { get; set; }
    public int? ScoreMin { get; set; }
    public int? ScoreMax { get; set; }
    public bool? IsViewedByRecruiter { get; set; }
    
    public bool? LoadVacancy { get; set; }
    public bool? LoadUser { get; set; }
}