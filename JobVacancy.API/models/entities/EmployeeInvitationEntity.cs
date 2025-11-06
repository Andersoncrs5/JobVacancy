using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.entities;

public class EmployeeInvitationEntity: BaseEntity
{
    public string UserId {get; set;}
    public virtual UserEntity? User {get; set;}
    public string EnterpriseId {get; set;}
    public virtual EnterpriseEntity? Enterprise {get; set;}
    public string? Message {get; set;} 
    public string? RejectReason {get; set;} 
    public string? InvitationLink {get; set;} 
    public string? Token {get; set;} 
    public string Position {get; set;} = string.Empty;
    public string SalaryRange {get; set;} = string.Empty;
    public EmploymentTypeEnum EmploymentType {get; set;}
    public DateTime ProposedStartDate {  get; set; }
    public DateTime? ProposedEndDate { get; set; }
    public StatusEnum Status { get; set; }
    public CurrencyEnum Currency { get; set; }
    public DateTime? ResponseDate { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string? InviteSenderId  {get; set;}
    public virtual UserEntity? InviteSender {get; set;}
}