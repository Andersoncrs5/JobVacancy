using System.Text.Json.Serialization;
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
    public virtual ICollection<UserSkillEntity>? UserSkill { get; set; } 
    
}