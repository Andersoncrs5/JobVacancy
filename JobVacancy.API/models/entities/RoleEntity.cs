using Microsoft.AspNetCore.Identity;

namespace JobVacancy.API.models.entities;

public class RoleEntity: IdentityRole
{
    public string? Description { get; set; }
}