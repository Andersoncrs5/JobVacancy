using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.Utils.Filters;

namespace JobVacancy.API.models.dtos.EnterpriseFollowsUser;

public class EnterpriseFollowsUserDto: FilterBaseParams
{
    public required string EnterpriseId { get; set; }
    public EnterpriseDto? Enterprise { get; set; }
    
    public required string UserId { get; set; }
    public UserDto? User { get; set; }

    public bool WishReceiveNotifyByNewPost { get; set; } 
    public bool WishReceiveNotifyByNewEndorsement { get; set; } 
    public bool WishReceiveNotifyByProfileUpdate { get; set; }
    public bool WishReceiveNotifyByNewInteraction { get; set; }
}