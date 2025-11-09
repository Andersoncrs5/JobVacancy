using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Annotations.Validations;

namespace JobVacancy.API.models.dtos.EmployeeEnterprise;

public class CreateEmployeeEnterpriseDto
{
    [ExistsUserById]
    public string UserId {get; set;} = string.Empty;
    public string? ContractLink {get; set;}
    public string SalaryRange {get; set;} = string.Empty;
    public string? Notes {get; set;}
    public decimal SalaryValue {get; set;}
    public PaymentFrequencyEnum PaymentFrequency {get; set;}
    public ContractLegalTypeEnum? ContractLegalType {get; set;}
    public EmploymentTypeEnum ContractType {get; set;} 
    public EmploymentTypeEnum EmploymentType {get; set;}
    public EmploymentStatusEnum EmploymentStatus {get; set;}
    public CurrencyEnum Currency { get; set; }
    public DateTime StartDate {get; set;}
    public DateTime? EndDate {get; set;}
    
    [ExistsPosition]
    public string PositionId {get; set;} = string.Empty;
    public string? InviteSenderId  {get; set;}
}