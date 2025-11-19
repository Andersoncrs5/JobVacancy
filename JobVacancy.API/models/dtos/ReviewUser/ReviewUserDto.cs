using JobVacancy.API.models.dtos.Base;
using JobVacancy.API.models.dtos.Users;

namespace JobVacancy.API.models.dtos.ReviewUser;

public class ReviewUserDto: ReviewBaseDto
{

    public bool? Recommendation { get; set; }

    public required string ActorId { get; set; }
    public UserDto? Actor { get; set; }

    public required string TargetUserId { get; set; }
    public UserDto? TargetUser { get; set; }
}