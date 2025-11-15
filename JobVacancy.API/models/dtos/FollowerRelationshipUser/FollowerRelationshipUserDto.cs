using JobVacancy.API.models.dtos.Users;

namespace JobVacancy.API.models.dtos.FollowerRelationshipUser;

public class FollowerRelationshipUserDto: BaseDto
{
    public required string FollowerId { get; set; }
    public UserDto? Follower { get; set; }

    public required string FollowedId { get; set; }
    public UserDto? Followed { get; set; }

    public bool WishReceiveNotifyByNewPost { get; set; } 
    public bool WishReceiveNotifyByNewComment { get; set; }
    public bool WishReceiveNotifyByNewInteraction { get; set; }
}