using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.entities;

public class BaseEntity
{
    [MaxLength(450)] public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}