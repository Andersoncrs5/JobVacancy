namespace JobVacancy.API.models.entities;

public class PositionEntity: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Describe { get; set; }
    public bool IsActive { get; set; }
    public virtual ICollection<EmployeeInvitationEntity>  EmployeeInvitations { get; set; } = new List<EmployeeInvitationEntity>();
    public virtual ICollection<EmployeeEnterpriseEntity> EmployeeEnterprise { get; set; } = new List<EmployeeEnterpriseEntity>();
    
}