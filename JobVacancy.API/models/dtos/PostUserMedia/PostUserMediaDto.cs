using JobVacancy.API.models.dtos.Base;
using JobVacancy.API.models.dtos.PostUser;

namespace JobVacancy.API.models.dtos.PostUserMedia;

public class PostUserMediaDto: MediaBase
{
    public required string PostId { get; set; }
    public PostUserDto? PostUser { get; set; }
}