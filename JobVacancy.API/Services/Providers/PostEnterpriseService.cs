using AutoMapper;
using JobVacancy.API.models.dtos.PostEnterprise;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class PostEnterpriseService(IUnitOfWork uow, IMapper mapper): IPostEnterpriseService
{
    public async Task<PostEnterpriseEntity?> GetById(string id) 
    {
        return await uow.PostEnterpriseRepository.GetByIdAsync(id);
    }

    public async Task<bool> ExistsById(string id) 
    {
        return await uow.PostEnterpriseRepository.ExistsById(id);
    }

    public async Task Delete(PostEnterpriseEntity entity) 
    {
        uow.PostEnterpriseRepository.Delete(entity);
        await uow.Commit();
    }

    public async Task<PostEnterpriseEntity> Create(CreatePostEnterpriseDto dto, string enterpriseId)
    {
        PostEnterpriseEntity map = mapper.Map<PostEnterpriseEntity>(dto);
        map.EnterpriseId = enterpriseId;
        PostEnterpriseEntity newPost = await uow.PostEnterpriseRepository.AddAsync(map);
        await uow.Commit();
        return newPost;
    }
    
    public IQueryable<PostEnterpriseEntity> Query() 
    {
        return uow.PostEnterpriseRepository.Query();
    }

    public async Task<PostEnterpriseEntity> Update(UpdatePostEnterpriseDto dto, PostEnterpriseEntity postEnterprise) 
    {
        mapper.Map(dto, postEnterprise);
        PostEnterpriseEntity update = await uow.PostEnterpriseRepository.Update(postEnterprise);
        await uow.Commit();
        return update;
    }
}