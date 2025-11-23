using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class PostUserMetricsService(IUnitOfWork uow): IPostUserMetricsService
{
    public async Task<PostUserMetricsEntity?> GetById(string id)
        => await uow.PostUserMetricsRepository.GetByIdAsync(id);
    
    public async Task<bool> ExistsById(string id)
        => await uow.PostUserMetricsRepository.ExistsById((id));
    
    public async Task<PostUserMetricsEntity?> GetByPostId(string id)
        => await uow.PostUserMetricsRepository.GetByPostId(id);
    
    public async Task<bool> ExistsByPostId(string id)
        => await uow.PostUserMetricsRepository.ExistsByPostId(id);
    
    public IQueryable<PostUserMetricsEntity> Query()
        => uow.PostUserMetricsRepository.Query();

    public async Task DeleteAsync(PostUserMetricsEntity entity)
    {
        await uow.PostUserMetricsRepository.DeleteAsync(entity);
        await uow.Commit();
    }

    public async Task<PostUserMetricsEntity> Create(string postId) 
    {
        PostUserMetricsEntity entity = new PostUserMetricsEntity()
        {
            PostId = postId
        };

        PostUserMetricsEntity async = await uow.PostUserMetricsRepository.AddAsync(entity);
        await uow.Commit();
        
        return async;
    }

    
}