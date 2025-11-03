namespace JobVacancy.API.models.entities;

public class PostUserEntity: BasePostTable
{
    public string UserId { get; set; }
    public virtual UserEntity? User { get; set; }
    public string CategoryId { get; set; }
    public virtual CategoryEntity? Category { get; set; }
    public virtual ICollection<FavoritePostUserEntity> FavoritePosts { get; set; } = new List<FavoritePostUserEntity>();
    public virtual ICollection<CommentPostUserEntity> CommentPostUser { get; set; } = new List<CommentPostUserEntity>();
}