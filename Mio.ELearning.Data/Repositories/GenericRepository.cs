using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Mio.Elearning.Core.Models;
using Mio.ELearning.Core.Repositories;

namespace Mio.ELearning.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
{
    protected readonly LMSDbContext _context;

    public GenericRepository(LMSDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
    {
        return await _context.Set<T>()
            .Where(e => e.IsActive && !e.IsDeleted)
            .Where(expression)
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>()
            .Where(e => e.IsActive && !e.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter, string includeProperties = "")
    {
        IQueryable<T> query = _context.Set<T>()
            .Where(e => e.IsActive && !e.IsDeleted)
            .Where(filter);

        if (!string.IsNullOrWhiteSpace(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity == null || !entity.IsActive || entity.IsDeleted)
        {
            return null;
        }
        return entity;
    }

    public void Remove(T entity)
    {
        entity.IsDeleted = true;
        entity.IsActive = false;
        entity.UpdatedAt = DateTime.Now;
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.IsActive = false;
            entity.UpdatedAt = DateTime.Now;
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
