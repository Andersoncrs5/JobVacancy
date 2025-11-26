using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Base;

namespace JobVacancy.API.models.entities;

public class PostUserMediaEntity: MediaBaseEntity
{
    [MaxLength(450)] public required string PostId { get; set; }
    public PostUserEntity? Post { get; set; }
}