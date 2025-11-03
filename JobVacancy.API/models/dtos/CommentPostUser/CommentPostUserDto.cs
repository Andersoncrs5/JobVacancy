using JobVacancy.API.models.dtos.Base;
using JobVacancy.API.models.dtos.PostUser;

namespace JobVacancy.API.models.dtos.CommentPostUser;

public class CommentPostUserDto: CommentBase
{
    public string PostId { get; set; } = string.Empty;
    public PostUserDto? Post { get; set; }
}