using JobVacancy.API.models.dtos.PostUserMedia;
using JobVacancy.API.models.entities;
using Minio.DataModel.Response;

namespace JobVacancy.API.Services.Interfaces;

public interface IPostUserMediaService
{
    Task<PostUserMediaEntity?> GetById(string postId);
    Task<bool> ExistsById(string postId);
    IQueryable<PostUserMediaEntity> Query();
    Task<int> TotalMediaByPost(string postId);
    Task Delete(PostUserMediaEntity entity);
    Task<PostUserMediaEntity> Create(CreatePostUserMediaDto media, PutObjectResponse response, string bucketName);
}