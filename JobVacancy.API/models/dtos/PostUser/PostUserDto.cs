using JobVacancy.API.models.dtos.Users;

namespace JobVacancy.API.models.dtos.PostUser;

public class PostUserDto: BasePost
{
    public string UserId { get; set; } = string.Empty;
    public virtual UserDto? User { get; set; }
}