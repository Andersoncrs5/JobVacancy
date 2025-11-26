using JobVacancy.API.models.dtos.Resume;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;
using Minio.DataModel.Response;

namespace JobVacancy.API.Services.Providers;

public class ResumeService(IUnitOfWork uow): IResumeService
{
    public async Task<bool> ExistsByName(string name)
        => await uow.ResumeRepository.ExistsByName(name);
    
    public async Task<ResumeEntity?> GetByName(string name)
        => await uow.ResumeRepository.GetByName(name);

    public async Task<bool> ExistsById(string id)
        => await uow.ResumeRepository.ExistsById(id);
    
    public async Task<ResumeEntity?> GetById(string id)
        => await uow.ResumeRepository.GetByIdAsync(id);

    
    public async Task<bool> ExistsByObjectKey(string key)
        => await uow.ResumeRepository.ExistsByObjectKey(key);

    public async Task<ResumeEntity?> GetByObjectKey(string key)
        => await uow.ResumeRepository.GetByObjectKey(key);
    
    public async Task<bool> ExistsByUrl(string url)
        => await uow.ResumeRepository.ExistsByUrl(url);
    
    public async Task<ResumeEntity?> GetByUrl(string url)
        => await uow.ResumeRepository.GetByUrl(url);
    
    public IQueryable<ResumeEntity> Query()
        => uow.ResumeRepository.Query();

    public async Task Delete(ResumeEntity entity)
    {
        await uow.ResumeRepository.DeleteAsync(entity);
        await uow.Commit();
    }

    public async Task<ResumeEntity> Create(CreateResumeDto dto, string userId, PutObjectResponse response)
    {
        ResumeEntity map = uow.Mapper.Map<ResumeEntity>(dto);
        map.userId = userId;
        map.BucketName = "resumes";
        map.ObjectKey = response.ObjectName;
        
        ResumeEntity async = await uow.ResumeRepository.AddAsync(map);
        await uow.Commit();
        return async;
    }
    
}