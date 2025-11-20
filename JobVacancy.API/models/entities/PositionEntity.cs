namespace JobVacancy.API.models.entities;

public class PositionEntity: BaseEntity
{
    public required string Name { get; set; }
    public string? Describe { get; set; }
    public required bool IsActive { get; set; }
    public ICollection<EmployeeInvitationEntity>?  EmployeeInvitations { get; set; }
    public ICollection<EmployeeEnterpriseEntity>? EmployeeEnterprise { get; set; }
    public ICollection<ReviewEnterpriseEntity>? Reviews { get; set; }
    public ICollection<UserEvaluationEntity>? Evaluations { get; init; }
}