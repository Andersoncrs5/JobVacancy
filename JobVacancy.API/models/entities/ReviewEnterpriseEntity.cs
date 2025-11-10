namespace JobVacancy.API.models.entities;

public class ReviewEnterpriseEntity: BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int RatingOverall { get; set; }
    
    public int? RatingCulture { get; set; }
    public int? RatingCompensation { get; set; }
    public int? RatingManagement { get; set; }
    public int? RatingWorkLifeBalance { get; set; }
    
    public bool IsAnonymous { get; set; }
    
    public string PositionId { get; set; } = string.Empty;
    public PositionEntity? Position { get; set; }
    
    public string UserId { get; set; } = string.Empty;
    public UserEntity? User { get; set; }
    
    public string EnterpriseId { get; set; } = string.Empty;
    public EnterpriseEntity? Enterprise { get; set; }
    
}