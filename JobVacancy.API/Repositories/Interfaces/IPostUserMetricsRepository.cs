using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IPostUserMetricsRepository: IGenericRepository<PostUserMetricsEntity>
{
    Task<PostUserMetricsEntity?> GetByPostId(string postId);
    Task<bool> ExistsByPostId(string postId);
}