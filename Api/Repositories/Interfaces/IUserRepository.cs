using Api.models.entities;
using Microsoft.AspNetCore.Identity;

namespace Api.Repositories.Interfaces;

public interface IUserRepository
{
    Task<UserEntity?> GetById(string id);
    Task<UserEntity?> GetByEmail(string email);
    Task<UserEntity?> GetByUsername(string username);
    Task<IdentityResult> Delete(UserEntity user);
    Task<IdentityResult> Insert(UserEntity user);
    Task<IdentityResult> Update(UserEntity user);
    Task<bool> ExistsByEmail(string email);
    Task<bool> ExistsByUsername(string username);

}