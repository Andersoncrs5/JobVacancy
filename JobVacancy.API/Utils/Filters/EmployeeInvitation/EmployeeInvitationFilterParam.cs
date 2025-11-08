using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.Utils.Filters.EmployeeInvitation;

public class EmployeeInvitationFilterParam: FilterBaseParams
{
    public string? UserId {get; set;}
    public string? NameUser {get; set;}
    public string? EmailUser {get; set;}
    
    public string? EnterpriseId {get; set;}
    public string? NameEnterprise {get; set;}
    public EnterpriseTypeEnum? TypeEnterprise {get; set;}

    public string? SalaryRange {get; set;}
    public EmploymentTypeEnum? EmploymentType {get; set;}

    public DateTime? ProposedStartDateMin {get; set;}
    public DateTime? ProposedStartDateMax {get; set;}
    public DateTime? ProposedEndDateMin {get; set;}
    public DateTime? ProposedEndDateMax {get; set;}
    public DateTime? ResponseDateMin {get; set;}
    public DateTime? ResponseDateMax {get; set;}
    public CurrencyEnum? Currency { get; set; }
    public StatusEnum? Status {get; set;}

    public string? InviteId {get; set;}
    public string? NameInvite {get; set;}
    public string? EmailInvite {get; set;}

    public string? PositionId {get; set;}
    public string? NamePosition {get; set;}
}