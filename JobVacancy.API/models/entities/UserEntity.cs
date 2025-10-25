using Microsoft.AspNetCore.Identity;

namespace JobVacancy.API.models.entities;

public class UserEntity: IdentityUser
{
    public string? FullName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}