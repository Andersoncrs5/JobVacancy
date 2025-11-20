using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.entities;

public class EmployeeInvitationEntity: BaseEntity
{
    [MaxLength(450)] public required string UserId {get; set;}
    public UserEntity? User {get; set;}
    
    [MaxLength(450)] public required string EnterpriseId {get; set;}
    public EnterpriseEntity? Enterprise {get; set;}
    
    public string? Message {get; set;} 
    public string? RejectReason {get; set;} 
    public string? InvitationLink {get; set;} 
    public string? Token {get; set;} 
    public string SalaryRange {get; set;} = string.Empty;
    public EmploymentTypeEnum EmploymentType {get; set;}
    public DateTime ProposedStartDate {  get; set; }
    public DateTime? ProposedEndDate { get; set; }
    public StatusEnum Status { get; set; }
    public CurrencyEnum Currency { get; set; }
    public DateTime? ResponseDate { get; set; }
    public DateTime ExpiresAt { get; set; }
    
    [MaxLength(450)] public string? InviteSenderId  {get; set;}
    public UserEntity? InviteSender {get; set;}
    
    [MaxLength(450)] public required string PositionId {get; set;}
    public PositionEntity? Position { get; set; }
}