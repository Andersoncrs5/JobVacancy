using JobVacancy.API.models.dtos;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities;
using JobVacancy.API.Services.Interfaces;
using JobVacancy.API.Utils.Facades;
using JobVacancy.API.Utils.Mappers;
using JobVacancy.API.Utils.Uow.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace JobVacancy.API.Services.Providers;

public class UserService(
    IUnitOfWork uow,
    IPasswordHasher<UserEntity> passwordHasher,
    IMapper mapper): IUserService
{
    private UserResult ReturnResult(IdentityResult result, UserEntity? user)
    {
        if (result.Succeeded)
        {
            
            return new UserResult
            {
                Succeeded = true,
                User = user,
                Errors = null
            };
        }
        else
        {
            return new UserResult
            {
                Succeeded = false,
                User = user,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }
    }

    public async Task<UserEntity?> GetUserBySid(string sid)
    {
        return await uow.UserRepository.GetById(sid);
    }

    public async Task<bool> CheckPassword(UserEntity user, string password)
    {
        return await uow.UserRepository.CheckPassword(user, password);
    }
    
    public async Task<UserResult> DeleteUser(UserEntity user)
    {
         var result = await uow.UserRepository.Delete(user);
         if (result.Succeeded)
             await uow.Commit(); 
         
         return ReturnResult(result, null);
    }
    
    public async Task<UserEntity?> GetUserByEmail(string email)
    {
        return await uow.UserRepository.GetByEmail(email);
    }
    
    public async Task<UserEntity?> GetUserByUsername(string username)
    {
        return await uow.UserRepository.GetByUsername(username);
    }
    
    public async Task<UserEntity?> GetUserByRefreshToken(string refreshToken)
    {
        return await uow.UserRepository.GetByRefreshToken(refreshToken);
    }

    public async Task<UserResult> CreateAsync(CreateUserDto dto)
    {
        UserEntity user = mapper.Map<UserEntity>(dto);

        var result = await uow.UserRepository.Insert(user);
        if (result.Succeeded)
            await uow.Commit();

        var userCreated = await GetUserByEmail(dto.Email);
        return ReturnResult(result, userCreated);
    }

    public async Task<UserResult> UpdateAsync(UserEntity user, UpdateUserDto dto)
    {
        if (dto.FullName != null)
            user.FullName = dto.FullName;
        
        user.UserName = dto.Name;
        
        if (!string.IsNullOrEmpty(dto.Password))
        {
            user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);
        }
        
        IdentityResult result = await uow.UserRepository.Update(user);
        
        if (result.Succeeded)
        {
            await uow.Commit(); 
        }

        var userChanged = await GetUserByEmail(user.Email!);

        return ReturnResult(result,userChanged);
    }

    public async Task<bool> ExistsByEmail(string email)
    {
        return await uow.UserRepository.ExistsByEmail(email);
    }
    
    public async Task<bool> ExistsByUsername(string username)
    {
        return await uow.UserRepository.ExistsByUsername(username);
    }

    public async Task<UserResult> AddRoleToUser(UserEntity user, RoleEntity role)
    {
        var result = await uow.UserRepository.AddRoleToUser(user, role);
        if (result.Succeeded)
            await uow.Commit();
        
        var userChanged = await GetUserByEmail(user.Email!);
        
        return ReturnResult(result, userChanged);
    }

    public async Task<IList<string>> GetRolesAsync(UserEntity user)
    {
        return await uow.UserRepository.GetRolesAsync(user);
    }

    public async Task<UserResult> UpdateSimple(UserEntity user)
    {
        IdentityResult result = await uow.UserRepository.Update(user);
        if (result.Succeeded)
            await uow.Commit();
        
        var userChanged = await GetUserByEmail(user.Email!);
        return ReturnResult(result, userChanged);
    }
    
}