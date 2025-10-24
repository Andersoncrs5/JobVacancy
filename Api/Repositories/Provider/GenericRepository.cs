using Api.Context;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Api.Repositories.Provider;

public class GenericRepository<T>: IGenericRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;
    
    public GenericRepository(AppDbContext context, DbSet<T> dbSet)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await this._dbSet.ToListAsync();
    }
    
    public IQueryable ReturnIQueryable()
    {
        return this._dbSet;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> AddAsync(T entity)
    {
        EntityEntry<T> data = await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return data.Entity;
    }

    public async void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public async void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}