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

public class UserService(
    IUnitOfWork uow,
    IPasswordHasher<UserEntity> passwordHasher,
    IMapper mapper): IUserService
{
    private UserResult ReturnResult(IdentityResult result)
    {
        if (result.Succeeded)
        {
            
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
         if (result.Succeeded)
             await uow.Commit(); 
         
         return ReturnResult(result);
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
        user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);

        var result = await uow.UserRepository.Insert(user);
        if (result.Succeeded)
            await uow.Commit(); 
        
        return ReturnResult(result);
    }

    public async Task<UserResult> UpdateAsync(UserEntity user, UpdateUserDto dto)
    {
        mapper.Map(dto, user); 

        if (!string.IsNullOrEmpty(dto.Password))
        {
            user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);
        }
        
        var result = await uow.UserRepository.Update(user);
        
        if (result.Succeeded)
        {
            await uow.Commit(); 
        }

        return ReturnResult(result);
    }
    
}