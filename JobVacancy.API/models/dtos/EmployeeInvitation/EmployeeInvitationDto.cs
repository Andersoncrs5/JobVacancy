using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.EmployeeInvitation;

public class EmployeeInvitationDto: BaseDto
{
    public string UserId {get; set;}
    public UserDto? User {get; set;}
    public string EnterpriseId {get; set;}
    public EnterpriseDto? Enterprise {get; set;}
    public string? Message {get; set;} 
    public string? RejectReason {get; set;}
    public string? InvitationLink {get; set;} 
    public string? Token {get; set;}
    public string PositionId {get; set;} = string.Empty;
    public string SalaryRange {get; set;} = string.Empty;
    public EmploymentTypeEnum EmploymentType {get; set;}
    public DateTime ProposedStartDate {  get; set; }
    public DateTime? ProposedEndDate { get; set; }
    public StatusEnum Status { get; set; }
    public CurrencyEnum Currency { get; set; }
    public DateTime? ResponseDate { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string? InviteSenderId  {get; set;}
    public UserDto? InviteSender {get; set;}
}