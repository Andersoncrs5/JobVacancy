using JobVacancy.API.models.entities;

namespace JobVacancy.API.Services.Interfaces;

public interface IPostUserMetricsService
{
    Task<PostUserMetricsEntity?> GetById(string id);
    Task<bool> ExistsById(string id);
    Task<PostUserMetricsEntity?> GetByPostId(string id);
    Task<bool> ExistsByPostId(string id);
    IQueryable<PostUserMetricsEntity> Query();
    Task DeleteAsync(PostUserMetricsEntity entity);
    Task<PostUserMetricsEntity> Create(string postId);
}