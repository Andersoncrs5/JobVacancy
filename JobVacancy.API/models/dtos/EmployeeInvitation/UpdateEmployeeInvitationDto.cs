using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.EmployeeInvitation;

public class UpdateEmployeeInvitationDto
{
    public string? UserId {get; set;} 
    public string? Message {get; set;}
    public string? Position {get; set;}
    public string? SalaryRange {get; set;}
    public EmploymentTypeEnum? EmploymentType {get; set;}
    public DateTime? ProposedStartDate {  get; set; }
    public DateTime? ProposedEndDate { get; set; }
    public StatusEnum? Status { get; set; }
    public CurrencyEnum? Currency { get; set; }
}