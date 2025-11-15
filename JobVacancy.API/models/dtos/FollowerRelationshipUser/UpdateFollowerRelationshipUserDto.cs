namespace JobVacancy.API.models.dtos.FollowerRelationshipUser;

public class UpdateFollowerRelationshipUserDto
{
    public bool? WishReceiveNotifyByNewPost { get; set; } 
    public bool? WishReceiveNotifyByNewComment { get; set; }
    public bool? WishReceiveNotifyByNewInteraction { get; set; }
}