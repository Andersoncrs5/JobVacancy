namespace JobVacancy.API.models.entities;

public class AreaEntity: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}