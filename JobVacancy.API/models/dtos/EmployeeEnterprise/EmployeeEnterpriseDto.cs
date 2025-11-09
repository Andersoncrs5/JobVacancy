using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Position;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.EmployeeEnterprise;

public class EmployeeEnterpriseDto: BaseDto
{
    public string UserId {get; set;}
    public UserDto? User {get; set;}
    public string EnterpriseId {get; set;}
    public EnterpriseDto? Enterprise {get; set;}
    public string? ContractLink {get; set;}
    public string SalaryRange {get; set;} = string.Empty;
    public string? TerminationReason {get; set;} 
    public string? Notes {get; set;}
    public decimal SalaryValue {get; set;}
    public PaymentFrequencyEnum PaymentFrequency {get; set;}
    public ContractLegalTypeEnum? ContractLegalType {get; set;}
    public CurrencyEnum SalaryCurrency { get; set; } 
    public EmploymentTypeEnum EmploymentType {get; set;}
    public EmploymentStatusEnum EmploymentStatus {get; set;}
    public CurrencyEnum Currency { get; set; }
    public DateTime StartDate {get; set;}
    public DateTime? EndDate {get; set;}
    public string PositionId {get; set;} = string.Empty;
    public PositionDto? Position {get; set;}
    public string? InviteSenderId  {get; set;}
    public UserDto? InviteSender {get; set;}
}