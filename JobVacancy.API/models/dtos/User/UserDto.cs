namespace JobVacancy.API.models.dtos.Users;

public class UserDto: BaseDto
{
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? FullName { get; set; }
    
}