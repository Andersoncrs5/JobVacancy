using Api.models.dtos;
using Api.models.dtos.Users;
using Api.models.entities;
using Api.Services.Interfaces;
using Api.Utils.Facades;
using Api.Utils.Mappers;
using Api.Utils.Uow.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Api.Services.Providers;

public class UserService(IUnitOfWork uow, IMapper mapper): IUserService
{
    private async Task<UserResult> ReturnResult(IdentityResult result)
    {
        if (result.Succeeded)
        {
            await uow.Commit();

            return new UserResult
            {
                Succeeded = true,
                User = null,
                Errors = null
            };
        }
        else
        {
            return new UserResult
            {
                Succeeded = false,
                User = null,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }
    }
    
    public async Task<UserResult> DeleteUser(UserEntity user)
    {
         var result = await uow.UserRepository.Delete(user);
         return await ReturnResult(result);
    }
    
    public async Task<UserEntity?> GetUserByEmail(string email)
    {
        return await uow.UserRepository.GetByEmail(email);
    }
    
    public async Task<UserEntity?> GetUserByUsername(string username)
    {
        return await uow.UserRepository.GetByUsername(username);
    }

    public async Task<UserResult> CreateAsync(CreateUserDto dto)
    {
        UserEntity user = mapper.Map<UserEntity>(dto);

        var result = await uow.UserRepository.Insert(user);

        return await ReturnResult(result);
    }
    
}