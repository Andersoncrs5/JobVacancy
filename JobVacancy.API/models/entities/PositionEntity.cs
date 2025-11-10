namespace JobVacancy.API.models.entities;

public class PositionEntity: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Describe { get; set; }
    public bool IsActive { get; set; }
    public ICollection<EmployeeInvitationEntity>?  EmployeeInvitations { get; set; }
    public ICollection<EmployeeEnterpriseEntity>? EmployeeEnterprise { get; set; }
    public ICollection<ReviewEnterpriseEntity>? Reviews { get; set; }
    
}