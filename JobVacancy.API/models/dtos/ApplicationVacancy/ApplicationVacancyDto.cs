using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.dtos.Vacancy;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.ApplicationVacancy;

public class ApplicationVacancyDto: BaseDto
{
    public required string VacancyId { get; set; }
    public VacancyDto? Vacancy { get; set; }
    
    public required string UserId { get; set; }
    public UserDto? User { get; set; }
    
    public ApplicationStatusEnum Status { get; set; }
    public DateTime? LastStatusUpdateDate { get; set; }
    
    public string? CoverLetter { get; set; }
    public int? Score { get; set; }
    
    public bool? IsViewedByRecruiter { get; set; }
}