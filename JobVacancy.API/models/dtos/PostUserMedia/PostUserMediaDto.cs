using JobVacancy.API.models.dtos.Base;
using JobVacancy.API.models.dtos.PostUser;

namespace JobVacancy.API.models.dtos.PostUserMedia;

public class PostUserMediaDto: MediaBase
{
    public required string PostId { get; init; }
    public PostUserDto? Post { get; init; }
}