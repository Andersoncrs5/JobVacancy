using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.EmployeeEnterprise;

public class UpdateEmployeeEnterpriseDto
{
    public string? ContractLink {get; set;}
    public string? SalaryRange {get; set;}
    public string? TerminationReason {get; set;} 
    public string? Notes {get; set;}
    public decimal? SalaryValue {get; set;}
    public PaymentFrequencyEnum? PaymentFrequency {get; set;}
    public ContractLegalTypeEnum? ContractLegalType {get; set;}
    public EmploymentTypeEnum? ContractType {get; set;} 
    public CurrencyEnum? SalaryCurrency { get; set; } 
    public EmploymentTypeEnum? EmploymentType {get; set;}
    public EmploymentStatusEnum? EmploymentStatus {get; set;}
    public CurrencyEnum? Currency { get; set; }
    public DateTime? StartDate {get; set;}
    public DateTime? EndDate {get; set;}
}