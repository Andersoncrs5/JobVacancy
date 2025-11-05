namespace JobVacancy.API.models.entities;

public class PostEnterpriseEntity: BasePostTable
{
    public string EnterpriseId { get; set; } = string.Empty;
    public virtual EnterpriseEntity? Enterprise { get; set; }
    public string CategoryId { get; set; } = string.Empty;
    public virtual CategoryEntity? Category { get; set; }
    public virtual ICollection<FavoritePostEnterpriseEntity> FavoritePostsEnterprise { get; set; } = new List<FavoritePostEnterpriseEntity>();
    public virtual ICollection<CommentPostEnterpriseEntity> Comments { get; set; } = new List<CommentPostEnterpriseEntity>();
    
}