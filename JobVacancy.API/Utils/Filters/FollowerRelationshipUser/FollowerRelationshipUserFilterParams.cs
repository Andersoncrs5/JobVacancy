namespace JobVacancy.API.Utils.Filters.FollowerRelationshipUser;

public class FollowerRelationshipUserFilterParams: FilterBaseParams
{
    public string? FollowerId { get; set; }
    public string? UserNameFollower { get; set; }
    public string? EmailFollower { get; set; }
    public string? FullNameFollower { get; set; }
    
    public string? FollowedId { get; set; }
    public string? UserNameFollowed { get; set; }
    public string? EmailFollowed { get; set; }
    public string? FullNameFollowed { get; set; }

    public bool? WishReceiveNotifyByNewPost { get; set; }
    public bool? WishReceiveNotifyByNewComment { get; set; }
    public bool? WishReceiveNotifyByNewInteraction { get; set; }
    
    public bool? LoadFollower { get; set; }
    public bool? LoadFollowed { get; set; }
}