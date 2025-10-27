using JobVacancy.API.Context;
using JobVacancy.API.models.entities;
using JobVacancy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JobVacancy.API.Repositories.Provider;

public class GenericRepository<T>: IGenericRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext Context;
    private readonly DbSet<T> _dbSet;
    
    protected GenericRepository(AppDbContext ctx)
    {
        Context = ctx;
        _dbSet = Context.Set<T>();
    }

    public async Task<bool> ExistsById(string Id)
    {
        return await _dbSet
            .AnyAsync(c => c.Id == Id);
    }
    
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await this._dbSet.ToListAsync();
    }
    
    public IQueryable<T> ReturnIQueryable()
    {
        return this._dbSet;
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> AddAsync(T entity)
    {
        EntityEntry<T> data = await _dbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
        return data.Entity;
    }

    public async Task<T> Update(T entity)
    {
        EntityEntry<T> update = _dbSet.Update(entity);
        return update.Entity;
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await Context.SaveChangesAsync();
    }
}