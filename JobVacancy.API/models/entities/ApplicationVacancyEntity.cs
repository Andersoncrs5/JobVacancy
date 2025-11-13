using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.entities;

public class ApplicationVacancyEntity: BaseEntity
{
    public required string VacancyId { get; set; }
    public VacancyEntity? Vacancy { get; set; }
    
    public required string UserId { get; set; }
    public UserEntity? User { get; set; }
    
    public ApplicationStatusEnum Status { get; set; }
    public DateTime? LastStatusUpdateDate { get; set; }
    
    public string? CoverLetter { get; set; }
    public int? Score { get; set; }
    
    public bool? IsViewedByRecruiter { get; set; }
}