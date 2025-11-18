namespace JobVacancy.API.models.entities;

public class ReviewUserEntity: BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int RatingOverall { get; set; }
    
    public int? RatingCulture { get; set; }
    public int? RatingCompensation { get; set; }
    public int? RatingManagement { get; set; }
    public int? RatingWorkLifeBalance { get; set; }

    public bool? Recommendation { get; set; }
    public bool IsAnonymous { get; set; }

    public required string ActorId { get; set; }
    public UserEntity? Actor { get; set; }

    public required string TargetUserId { get; set; }
    public UserEntity? TargetUser { get; set; }
}