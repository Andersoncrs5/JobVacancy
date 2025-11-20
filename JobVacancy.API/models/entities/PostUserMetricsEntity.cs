namespace JobVacancy.API.models.entities;

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