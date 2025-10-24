using Api.models.entities;

namespace Api.models.dtos.Users;

public class UserResult
{
    public bool Succeeded { get; set; }
    public UserEntity? User { get; set; } 
    public IEnumerable<string>? Errors { get; set; } = new List<string>();
}