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
    
    public virtual EnterpriseEntity? Enterprise { get; set; }
    public virtual ICollection<PostUserEntity> Posts { get; set; } = new List<PostUserEntity>();
    public virtual ICollection<UserSkillEntity> UserSkill { get; set; } = new List<UserSkillEntity>();
    public virtual ICollection<FavoritePostUserEntity> FavoritePosts { get; set; } = new List<FavoritePostUserEntity>();
    public virtual ICollection<FavoritePostEnterpriseEntity> FavoritePostsEnterprise { get; set; } = new List<FavoritePostEnterpriseEntity>();
    public virtual ICollection<CommentBaseEntity>  Comments { get; set; } = new List<CommentBaseEntity>();
    
    public virtual ICollection<EmployeeInvitationEntity> InvitationsReceived { get; set; } = new List<EmployeeInvitationEntity>();
    
    public virtual ICollection<EmployeeInvitationEntity> InvitationsSent { get; set; } = new List<EmployeeInvitationEntity>();
}