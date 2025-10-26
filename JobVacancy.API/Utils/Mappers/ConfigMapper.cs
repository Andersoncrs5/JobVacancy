using JobVacancy.API.models.dtos;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities;
using AutoMapper;
using JobVacancy.API.Utils.Page;

namespace JobVacancy.API.Utils.Mappers;

public class ConfigMapper: Profile
{
    public ConfigMapper()
    {
        CreateMap<CreateUserDto, UserEntity>();
        CreateMap<UserEntity, UserDto>();
        CreateMap(typeof(PaginatedList<>), typeof(Page<>));
    }
}