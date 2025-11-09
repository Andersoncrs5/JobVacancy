namespace JobVacancy.API.models.entities;

public class CategoryEntity: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public ICollection<PostUserEntity>? Posts { get; set; }
    public ICollection<PostEnterpriseEntity>? PostsEnterprise { get; set; }
}