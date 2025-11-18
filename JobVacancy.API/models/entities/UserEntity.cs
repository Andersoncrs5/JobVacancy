using System.Text.Json.Serialization;
using JobVacancy.API.models.entities.Base;
using Microsoft.AspNetCore.Identity;

namespace JobVacancy.API.models.entities;

public class UserEntity: IdentityUser
{
    public string? FullName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public EnterpriseEntity? Enterprise { get; set; }
    public ICollection<PostUserEntity>? Posts { get; set; }
    public ICollection<UserSkillEntity>? UserSkill { get; set; }
    public ICollection<FavoritePostUserEntity>? FavoritePosts { get; set; }
    public ICollection<FavoritePostEnterpriseEntity>? FavoritePostsEnterprise { get; set; }
    public ICollection<CommentBaseEntity> ? Comments { get; set; }
    public ICollection<EmployeeInvitationEntity>? InvitationsReceived { get; set; }
    public ICollection<EmployeeInvitationEntity>? InvitationsSent { get; set; }
    public ICollection<EmployeeEnterpriseEntity>? InvitationsEnterprise { get; set; }
    public ICollection<EmployeeEnterpriseEntity>? Employee { get; set; }
    public ICollection<ReviewEnterpriseEntity>? Reviews { get; set; }
    public ICollection<IndicationUserEntity>? SentEndors { get; set; }
    public ICollection<IndicationUserEntity>? ReceivedEndors { get; set; }
    public ICollection<ApplicationVacancyEntity>? ApplicationVacancies { get; set; }
    
    public ICollection<FollowerRelationshipUserEntity>? Followers { get; set; }
    public ICollection<FollowerRelationshipUserEntity>? Following { get; set; }
    public ICollection<FollowerUserRelationshipEnterpriseEntity>? FollowingEnterprise { get; init; }
    public ICollection<EnterpriseFollowsUserEntity>? FollowedByEnterprises { get; init; }
    public ICollection<ReviewUserEntity>? ReviewsWritten { get; set; }
    public ICollection<ReviewUserEntity>? ReviewsReceived { get; set; }
    
    
}