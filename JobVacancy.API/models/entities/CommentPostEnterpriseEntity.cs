using JobVacancy.API.models.entities.Base;

namespace JobVacancy.API.models.entities;

public class CommentPostEnterpriseEntity: CommentBaseEntity
{
    public string PostId { get; set; }
    public PostEnterpriseEntity? Post { get; set; }
}