namespace JobVacancy.API.models.entities;

public enum PostUserMetricsColumns
{
    LikeCount = 0,
    DislikeCount = 1,
    CommentCount = 2,
    FavoriteCount = 3,
    RepublishedCount = 4,
    SharedCount = 5
}

public class PostUserMetricsEntity: BaseEntity
{
    public long LikeCount { get; set; }
    public long DislikeCount { get; set; }
    public long CommentCount { get; set; }
    public long FavoriteCount { get; set; }
    public long RepublishedCount { get; set; }
    public long SharedCount { get; set; }
    
    public required string PostId { get; set; } 
    public PostUserEntity? Post { get; set; }
}