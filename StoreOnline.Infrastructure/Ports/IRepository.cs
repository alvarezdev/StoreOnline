using StoreOnline.Domain.Entities;
using System.Linq.Expressions;

namespace StoreOnline.Infrastructure.Ports;

public interface IRepository<T> where T : DomainEntity
{
    Task<T> GetOneAsync(Guid id);
    Task<IEnumerable<T>> GetManyAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeStringProperties = "",
        bool isTracking = false);
    Task<T> AddAsync(T entity);
    T Update(T entity);
    void Delete(T entity);

}
