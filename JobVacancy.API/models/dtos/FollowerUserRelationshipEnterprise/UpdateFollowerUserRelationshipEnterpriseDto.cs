namespace JobVacancy.API.models.dtos.FollowerUserRelationshipEnterprise;

public class UpdateFollowerUserRelationshipEnterpriseDto
{
    public bool? WishReceiveNotifyByNewPost { get; set; } 
    public bool? WishReceiveNotifyByNewVacancy { get; set; } 
    public bool? WishReceiveNotifyByNewComment { get; set; }
    public bool? WishReceiveNotifyByNewInteraction { get; set; }
}