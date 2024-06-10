using Halood.Domain.Entities;
using Halood.Domain.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Halood.Repository.EntityFramework;

namespace Halood.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
{
    protected readonly HaloodDbContext Context;

    public BaseRepository(HaloodDbContext context)
    {
        Context = context;
    }

    public virtual async Task CommitAsync()
    {
        await Context.SaveChangesAsync();
    }

    public virtual T Get(long id, bool asNoTracking = true)
    {
        var query = asNoTracking
            ? Context.Set<T>().AsNoTracking().FirstOrDefault(x => x.Id == id)
            : Context.Set<T>().Find(id);
        return query;
    }

    public virtual async Task<T> GetAsync(long id, bool asNoTracking = true)
    {
        var query = asNoTracking
            ? Context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id)
            : Context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

        return await query;
    }


    public virtual async Task<List<T>> SaveRangeAsync(List<T> entities)
    {
        await Context.Set<T>().AddRangeAsync(entities);
        return entities;
    }

    public virtual List<T> GetAll(bool asNoTracking = true)
    {
        return asNoTracking
            ? Context.Set<T>().AsNoTracking().ToList()
            : Context.Set<T>().ToList();
    }

    public virtual async Task<List<T>> GetAllAsync(bool asNoTracking = true)
    {
        return await (asNoTracking
            ? Context.Set<T>().AsNoTracking().ToListAsync()
            : Context.Set<T>().ToListAsync());
    }

    public virtual List<T> GetListBy(Expression<Func<T, bool>> predicate, bool asNoTracking = true)
    {
        var query = asNoTracking
            ? Context.Set<T>().Where(predicate).AsNoTracking().ToList()
            : Context.Set<T>().Where(predicate).ToList();
        return query;
    }

    public virtual async Task<List<T>> GetListByAsync(Expression<Func<T, bool>> predicate,
        bool asNoTracking = false)
    {
        var query = asNoTracking
            ? Context.Set<T>().Where(predicate).AsNoTracking().ToListAsync()
            : Context.Set<T>().Where(predicate).ToListAsync();
        return await query;
    }

    public virtual IQueryable<T> GetQueryableBy(Expression<Func<T, bool>> predicate, bool asNoTracking = false)
    {
        var query = asNoTracking
            ? Context.Set<T>().Where(predicate).AsNoTracking()
            : Context.Set<T>().Where(predicate);
        return query;
    }

    public virtual T Save(T entity)
    {
        Context.Set<T>().Add(entity);
        return entity;
    }

    public virtual async Task<T> SaveAsync(T entity)
    {
        await Context.Set<T>().AddAsync(entity);
        return entity;
    }

    public virtual List<T> SaveRange(List<T> entities)
    {
        Context.Set<T>().AddRange(entities);
        return entities;
    }

    public virtual T Update(T entity)
    {
        Context.Set<T>().Update(entity);
        return entity;
    }


    public virtual void Delete(T entity)
    {
        Context.Set<T>().Remove(entity);
    }

    public virtual void Delete(long id)
    {
        var entityToDelete = Context.Set<T>().FirstOrDefault(e => e.Id == id);
        if (entityToDelete != null)
        {
            Context.Set<T>().Remove(entityToDelete);
        }
    }

    public virtual void Commit()
    {
        Context.SaveChanges();
    }

    public virtual void Dispose()
    {
        Context?.Dispose();
    }
}
