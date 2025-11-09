using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.entities;

public class EmployeeEnterpriseEntity: BaseEntity
{
    public string UserId {get; set;}
    public UserEntity? User {get; set;}
    
    public string EnterpriseId {get; set;}
    public EnterpriseEntity? Enterprise {get; set;}
    
    public string? ContractLink {get; set;}
    public string SalaryRange {get; set;} = string.Empty;
    public string? TerminationReason {get; set;} 
    public string? Notes {get; set;}
    
    public decimal SalaryValue {get; set;}

    public PaymentFrequencyEnum PaymentFrequency {get; set;}
    public ContractLegalTypeEnum? ContractLegalType {get; set;}
    public EmploymentTypeEnum EmploymentType {get; set;}
    public EmploymentStatusEnum EmploymentStatus {get; set;}
    public CurrencyEnum Currency { get; set; }
    
    public DateTime StartDate {get; set;}
    public DateTime? EndDate {get; set;}
    
    public string PositionId {get; set;} = string.Empty;
    public PositionEntity? Position { get; set; }
    
    public string? InviteSenderId  {get; set;}
    public UserEntity? InviteSender {get; set;}
}