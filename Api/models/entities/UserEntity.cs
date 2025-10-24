using Microsoft.AspNetCore.Identity;

namespace Api.models.entities;

public class UserEntity: IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}