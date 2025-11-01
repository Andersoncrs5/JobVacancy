namespace JobVacancy.API.models.entities;

public class PostEnterpriseEntity: BasePostTable
{
    public string EnterpriseId { get; set; } = string.Empty;
    public virtual EnterpriseEntity Enterprise { get; set; } = new EnterpriseEntity();
    public string CategoryId { get; set; } = string.Empty;
    public virtual CategoryEntity? Category { get; set; }
    
}