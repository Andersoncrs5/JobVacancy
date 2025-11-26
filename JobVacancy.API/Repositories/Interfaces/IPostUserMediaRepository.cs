using JobVacancy.API.models.entities;

namespace JobVacancy.API.Repositories.Interfaces;

public interface IPostUserMediaRepository: IGenericRepository<PostUserMediaEntity>
{
    Task<int> TotalMediaByPost(string postId);
}