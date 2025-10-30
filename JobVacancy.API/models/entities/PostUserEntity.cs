namespace JobVacancy.API.models.entities;

public class PostUserEntity: BasePostTable
{
    public string UserId { get; set; }
    public virtual UserEntity? User { get; set; }
    public string CategoryId { get; set; }
    public virtual CategoryEntity? Category { get; set; }
}