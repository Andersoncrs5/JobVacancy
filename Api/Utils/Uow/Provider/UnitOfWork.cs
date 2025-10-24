using Api.Context;
using Api.models.entities;
using Api.Repositories.Interfaces;
using Api.Repositories.Provider;
using Api.Utils.Uow.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Api.Utils.Uow.Provider;

public class UnitOfWork: IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly UserManager<UserEntity> _userManager;
    private UserRepository _userRepository;

    public IUserRepository UserRepository
        => _userRepository ??= new UserRepository(_context, _userManager);
    
    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this); 
    }
}