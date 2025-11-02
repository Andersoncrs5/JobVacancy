using JobVacancy.API.models.dtos.PostUser;
using JobVacancy.API.models.dtos.Users;

namespace JobVacancy.API.models.dtos.FavoritePost;

public class FavoritePostUserDto: BaseDto
{
    public string UserId { get; set; } = string.Empty;
    public virtual UserDto? User { get; set; }
    
    public string PostUserId { get; set; } = string.Empty;
    public virtual PostUserDto? PostUser { get; set; }
    
    public string? UserNotes { get; set; }
    public int? UserRating { get; set; }
}