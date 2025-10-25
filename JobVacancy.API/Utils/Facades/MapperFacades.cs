using JobVacancy.API.models.dtos;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities;
using AutoMapper;

namespace JobVacancy.API.Utils.Facades;

public class MapperFacades: IMapperFacades
{
    private readonly IMapper _mapper;

    public MapperFacades(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public UserEntity Map(CreateUserDto dto)
    {
        return _mapper.Map<UserEntity>(dto);
    }

}