using Halood.Domain.Entities;
using System.Linq.Expressions;

namespace Halood.Domain.Interfaces.Base;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity, new()
{
    void Commit();
    Task CommitAsync();

    TEntity Get(long id, bool asNoTracking = true);
    Task<TEntity> GetAsync(long id, bool asNoTracking = true);

    void Delete(long id);
    void Delete(TEntity entity);

    TEntity Save(TEntity entity);
    Task<TEntity> SaveAsync(TEntity entity);

    TEntity Update(TEntity entity);

    List<TEntity> SaveRange(List<TEntity> entities);
    Task<List<TEntity>> SaveRangeAsync(List<TEntity> entities);

    List<TEntity> GetAll(bool asNoTracking = true);
    Task<List<TEntity>> GetAllAsync(bool asNoTracking = true);

    List<TEntity> GetListBy(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false);
    Task<List<TEntity>> GetListByAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false);

    IQueryable<TEntity> GetQueryableBy(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false);
}
