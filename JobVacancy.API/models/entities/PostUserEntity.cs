namespace JobVacancy.API.models.entities;

public class PostUserEntity: BasePostTable
{
    public string UserId { get; set; }
    public UserEntity? User { get; set; }
    public string CategoryId { get; set; }
    public CategoryEntity? Category { get; set; }
    public ICollection<FavoritePostUserEntity>? FavoritePosts { get; set; }
    public ICollection<CommentPostUserEntity>? CommentPostUser { get; set; }
}