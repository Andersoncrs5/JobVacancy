using JobVacancy.API.models.entities.Base;

namespace JobVacancy.API.models.entities;

public class CommentPostUserEntity: CommentBaseEntity
{
    public string PostId { get; set; }
    public PostUserEntity? Post { get; set; }
}