using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Annotations.Validations.Area;

namespace JobVacancy.API.models.dtos.Vacancy;

public class CreateVacancyDto
{
    [Required]
    [MaxLength(300)]
    public string Title { get; set; }
    
    [Required]
    [MaxLength(3000)]
    public string Description { get; set; }
    [MaxLength(1500)]
    public string? Requirements { get; set; }
    [MaxLength(1500)]
    public string? Responsibilities { get; set; }
    [MaxLength(1500)]
    public string? Benefits { get; set; }
    
    [Required]
    public EmploymentTypeEnum EmploymentType { get; set; }
    public ExperienceLevelEnum? ExperienceLevel { get; set; }
    public EducationLevelEnum? EducationLevel { get; set; }
    public WorkplaceTypeEnum? WorkplaceType { get; set; }
    public CurrencyEnum? Currency { get; set; }
    
    [Range(0, 10)]
    public int? Seniority { get; set; }
    [Range(0, 20)]
    public int Opening { get; set; }

    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    
    [Required]
    [ExistsAreaById]
    public string AreaId { get; set; }
    
    [DataType(DataType.DateTime)]
    public DateTime? ApplicationDeadLine { get; set; }
}