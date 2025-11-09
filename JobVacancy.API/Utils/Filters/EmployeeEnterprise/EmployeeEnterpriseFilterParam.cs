using JobVacancy.API.models.dtos;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.Utils.Filters.EmployeeEnterprise;

public class EmployeeEnterpriseFilterParam: FilterBaseParams
{
    public string? UserId {get; set;}
    public string? Username {get; set;}
    public string? UserEmail {get; set;}
    
    public string? EnterpriseId {get; set;}
    public string? NameEnterprise { get; set; }
    public EnterpriseTypeEnum? Type { get; set; }
    
    public string? SalaryRange { get; set; }
    public decimal? SalaryValueMin { get; set; }
    public decimal? SalaryValueMax { get; set; }
    
    public PaymentFrequencyEnum? PaymentFrequency {get; set;}
    public ContractLegalTypeEnum? ContractLegalType {get; set;}
    public EmploymentTypeEnum? EmploymentType {get; set;}
    public EmploymentStatusEnum? EmploymentStatus {get; set;}
    public CurrencyEnum? Currency { get; set; }
    
    public DateTime? StartDateMin {get; set;}
    public DateTime? StartDateMax {get; set;}
    public DateTime? EndDateMin {get; set;}
    public DateTime? EndDateMax {get; set;}
    
    public string? PositionId {get; set;}
    public string? NamePosition {get; set;}
    
    public string? InviteId {get; set;}
    public string? InviteName {get; set;}
    public string? InviteEmail {get; set;}
}