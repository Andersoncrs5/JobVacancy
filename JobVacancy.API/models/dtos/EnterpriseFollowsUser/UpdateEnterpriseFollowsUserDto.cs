namespace JobVacancy.API.models.dtos.EnterpriseFollowsUser;

public class UpdateEnterpriseFollowsUserDto
{
    public bool? WishReceiveNotifyByNewPost { get; set; } 
    public bool? WishReceiveNotifyByNewEndorsement { get; set; } 
    public bool? WishReceiveNotifyByProfileUpdate { get; set; }
    public bool? WishReceiveNotifyByNewInteraction { get; set; }
}