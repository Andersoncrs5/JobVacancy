using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.dtos;

namespace JobVacancy.API.models.entities;

public class FollowerRelationshipUserEntity: BaseEntity
{
    [MaxLength(450)] public required string FollowerId { get; set; }
    public UserEntity? Follower { get; set; }
    
    [MaxLength(450)] public required string FollowedId { get; set; }
    public UserEntity? Followed { get; set; }
    
    public bool WishReceiveNotifyByNewPost { get; set; } = true; 
    public bool WishReceiveNotifyByNewComment { get; set; } = true;
    public bool WishReceiveNotifyByNewInteraction { get; set; } = false;
}