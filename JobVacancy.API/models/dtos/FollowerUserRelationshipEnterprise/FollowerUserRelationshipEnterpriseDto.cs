using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Users;

namespace JobVacancy.API.models.dtos.FollowerUserRelationshipEnterprise;

public class FollowerUserRelationshipEnterpriseDto: BaseDto
{
    public required string UserId { get; set; }
    public UserDto? User { get; set; }
    
    public required string EnterpriseId { get; set; }
    public EnterpriseDto? Enterprise { get; set; }
    
    public bool WishReceiveNotifyByNewPost { get; set; } 
    public bool WishReceiveNotifyByNewVacancy { get; set; } 
    public bool WishReceiveNotifyByNewComment { get; set; }
    public bool WishReceiveNotifyByNewInteraction { get; set; }
}