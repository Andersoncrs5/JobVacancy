namespace JobVacancy.API.models.entities;

public class CategoryEntity: BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}