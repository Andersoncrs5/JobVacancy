using JobVacancy.API.models.dtos.PostUserMedia;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Minio.DataModel.Response;

namespace JobVacancy.API.Services.Providers;

public class PostUserMediaService(IUnitOfWork uow): IPostUserMediaService
{
    public async Task<PostUserMediaEntity?> GetById(string postId)
        => await uow.PostUserMediaRepository.GetByIdAsync(postId);
    
    public async Task<bool> ExistsById(string postId)
        => await uow.PostUserMediaRepository.ExistsById(postId);
    
    public IQueryable<PostUserMediaEntity> Query()
        => uow.PostUserMediaRepository.Query();

    public async Task<int> TotalMediaByPost(string postId)
        => await uow.PostUserMediaRepository.TotalMediaByPost(postId);
    
    public async Task Delete(PostUserMediaEntity entity)
    {
        await uow.PostUserMediaRepository.DeleteAsync(entity);
        await uow.Commit();
    }

    public async Task<PostUserMediaEntity> Create(CreatePostUserMediaDto media, PutObjectResponse response, string postId, string bucketName)
    {
        PostUserMediaEntity entity = new PostUserMediaEntity()
        {
            ObjectName = response.ObjectName,
            PostId = postId,
            FileSizeBytes = response.Size,
            Order = media.Order,
            BucketName = bucketName,
        };

        PostUserMediaEntity mediaEntity = await uow.PostUserMediaRepository.AddAsync(entity);
        await uow.Commit();

        return mediaEntity;
    }
    
}