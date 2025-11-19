using JobVacancy.API.models.dtos.Base;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Position;
using JobVacancy.API.models.dtos.Users;

namespace JobVacancy.API.models.dtos.ReviewEnterprise;

public class ReviewEnterpriseDto: ReviewBaseDto
{
    public string PositionId { get; set; } = string.Empty;
    public PositionDto? Position { get; set; }
    
    public string UserId { get; set; } = string.Empty;
    public UserDto? User { get; set; }
    
    public string EnterpriseId { get; set; } = string.Empty;
    public EnterpriseDto? Enterprise { get; set; }
}