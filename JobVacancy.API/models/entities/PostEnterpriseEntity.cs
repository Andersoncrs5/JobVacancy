namespace JobVacancy.API.models.entities;

public class PostEnterpriseEntity: BasePostTable
{
    public string EnterpriseId { get; set; } = string.Empty;
    public EnterpriseEntity? Enterprise { get; set; }
    public string CategoryId { get; set; } = string.Empty;
    public CategoryEntity? Category { get; set; }
    public ICollection<FavoritePostEnterpriseEntity>? FavoritePostsEnterprise { get; set; }
    public ICollection<CommentPostEnterpriseEntity>? Comments { get; set; }
    
}