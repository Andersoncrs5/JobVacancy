using JobVacancy.API.models.dtos.Area;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class AreaService(IUnitOfWork uow): IAreaService
{
    public async Task<AreaEntity?> GetById(string id)
        => await uow.AreaRepository.GetByIdAsync(id);

    public async Task<bool> ExistsById(string id)
        => await uow.AreaRepository.ExistsById(id);

    public async Task<bool> ExistsName(string name)
        => await uow.AreaRepository.ExistsByName(name);

    public async Task Delete(AreaEntity area)
    {
        uow.AreaRepository.Delete(area);
        await uow.Commit();
    }

    public async Task<AreaEntity> Create(CreateAreaDto dto)
    {
        AreaEntity map = uow.Mapper.Map<AreaEntity>(dto);

        AreaEntity newArea = await uow.AreaRepository.AddAsync(map);
        await uow.Commit();
        return newArea;
    }

    public async Task<AreaEntity> Update(UpdateAreaDto dto, AreaEntity area)
    {
        if (!string.IsNullOrWhiteSpace(dto.Description))
            area.Description = dto.Description;
        
        if (!string.IsNullOrWhiteSpace(dto.Name))
            area.Name = dto.Name;
        
        if (dto.IsActive.HasValue)
            area.IsActive = dto.IsActive.Value;

        AreaEntity update = await uow.AreaRepository.Update(area);
        await uow.Commit();
        return update;
    }

    public IQueryable<AreaEntity> Query()
    {
        return uow.AreaRepository.ReturnIQueryable();
    }
    
}