using Api.models.dtos;
using Api.models.dtos.Users;
using Api.models.entities;
using AutoMapper;

namespace Api.Utils.Facades;

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