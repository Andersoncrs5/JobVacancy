using Microsoft.AspNetCore.Identity;

namespace Api.models.entities;

public class RoleEntity: IdentityRole
{
    public string? Description { get; set; }
}