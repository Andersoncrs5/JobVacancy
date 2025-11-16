using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.Utils.Filters.FollowerUserRelationshipEnterprise;

public class FollowerUserRelationshipEnterpriseFilterParam: FilterBaseParams
{
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    
    public string? EnterpriseId { get; set; }
    public string? Name { get; set; }
    public EnterpriseTypeEnum? Type { get; set; }

    public bool? WishReceiveNotifyByNewPost { get; set; } 
    public bool? WishReceiveNotifyByNewVacancy { get; set; } 
    public bool? WishReceiveNotifyByNewComment { get; set; }
    public bool? WishReceiveNotifyByNewInteraction { get; set; }
    
    public bool? LoadUser { get; set; }
    public bool? LoadEnterprise { get; set; }
}