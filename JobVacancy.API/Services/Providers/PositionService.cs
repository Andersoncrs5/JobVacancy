using JobVacancy.API.models.dtos.Position;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Uow.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class PositionService(IUnitOfWork uow): IPositionService
{
    public async Task<PositionEntity?> GetById(string id)
    {
        return await uow.PositionRepository.GetByIdAsync(id);
    }

    public async Task<bool> ExistsByName(string name)
    {
        return await uow.PositionRepository.ExistsByName(name);
    }
    
    public async Task<bool> ExistsById(string id)
    {
        return await uow.PositionRepository.ExistsById(id);
    }

    public async Task Delete(PositionEntity position)
    {
        uow.PositionRepository.Delete(position);
        await uow.Commit();
    }
    
    public async Task<PositionEntity> Create(CreatePositionDto dto)
    {
        PositionEntity map = uow.Mapper.Map<PositionEntity>(dto);
        PositionEntity added = await uow.PositionRepository.AddAsync(map);
        
        await uow.Commit();
        return added;
    }

    public async Task<PositionEntity> Update(UpdatePositionDto dto, PositionEntity position)
    {
        var active = position.IsActive;
        
        uow.Mapper.Map(dto, position);

        if (dto.IsActive.HasValue)
        {
            position.IsActive = dto.IsActive.Value;
        }
        else
        {
            position.IsActive = active;
        }

        PositionEntity update = await uow.PositionRepository.Update(position);
        await uow.Commit();
        return update;
    }

    public IQueryable<PositionEntity> Query()
    {
        return uow.PositionRepository.ReturnIQueryable();
    }
    
}