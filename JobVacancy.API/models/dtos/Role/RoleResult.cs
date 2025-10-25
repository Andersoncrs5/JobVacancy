using JobVacancy.API.models.entities;

namespace JobVacancy.API.models.dtos.Role;

public class RoleResult
{
    public bool Succeeded { get; set; }
    public RoleEntity? User { get; set; } 
    public IEnumerable<string>? Errors { get; set; } = new List<string>();
}