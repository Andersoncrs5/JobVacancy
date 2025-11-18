using JobVacancy.API.models.dtos.Users;

namespace JobVacancy.API.models.dtos.ReviewUser;

public class ReviewUserDto: BaseDto
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
    public UserDto? Actor { get; set; }

    public required string TargetUserId { get; set; }
    public UserDto? TargetUser { get; set; }
}