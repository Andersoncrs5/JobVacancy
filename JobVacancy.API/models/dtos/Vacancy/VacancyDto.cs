using JobVacancy.API.models.dtos.Area;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.Vacancy;

public class VacancyDto: BaseDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    public string? Requirements { get; set; }
    public string? Responsibilities { get; set; }
    public string? Benefits { get; set; }

    public EmploymentTypeEnum EmploymentType { get; set; }
    public ExperienceLevelEnum? ExperienceLevel { get; set; }
    public EducationLevelEnum? EducationLevel { get; set; }
    public WorkplaceTypeEnum? WorkplaceType { get; set; }
    public CurrencyEnum? Currency { get; set; }
    public VacancyStatusEnum Status { get; set; }

    public int? Seniority { get; set; }
    public int Opening { get; set; }

    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }

    public string EnterpriseId { get; set; }
    public EnterpriseDto? Enterprise { get; set; }

    public string AreaId { get; set; }
    public AreaDto? Area { get; set; }

    public DateTime? ApplicationDeadLine { get; set; }
    public DateTime? LastApplication { get; set; } 
}