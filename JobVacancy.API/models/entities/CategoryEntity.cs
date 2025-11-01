namespace JobVacancy.API.models.entities;

public class CategoryEntity: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public virtual ICollection<PostUserEntity> Posts { get; set; } = new List<PostUserEntity>();
    public virtual ICollection<PostEnterpriseEntity> PostsEnterprise { get; set; } = new List<PostEnterpriseEntity>();
}