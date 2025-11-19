using JobVacancy.API.models.entities.Base;

namespace JobVacancy.API.models.entities;

public class ReviewEnterpriseEntity: ReviewBase
{
    public string PositionId { get; set; } = string.Empty;
    public PositionEntity? Position { get; set; }
    
    public string UserId { get; set; } = string.Empty;
    public UserEntity? User { get; set; }
    
    public string EnterpriseId { get; set; } = string.Empty;
    public EnterpriseEntity? Enterprise { get; set; }
    
}