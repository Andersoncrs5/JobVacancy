using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Position;
using JobVacancy.API.models.dtos.Users;

namespace JobVacancy.API.models.dtos.ReviewEnterprise;

public class ReviewEnterpriseDto: BaseDto
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
    public PositionDto? Position { get; set; }
    
    public string UserId { get; set; } = string.Empty;
    public UserDto? User { get; set; }
    
    public string EnterpriseId { get; set; } = string.Empty;
    public EnterpriseDto? Enterprise { get; set; }
}