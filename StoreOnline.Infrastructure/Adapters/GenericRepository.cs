using StoreOnline.Domain.Entities;
using StoreOnline.Infrastructure.DataSource;
using StoreOnline.Infrastructure.Ports;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace StoreOnline.Infrastructure.Adapters;

public class GenericRepository<T> : IRepository<T> where T : DomainEntity
{
    readonly DataContext Context;
    readonly DbSet<T> _dataset;

    public GenericRepository(DataContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        _dataset = Context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetManyAsync(
        Expression<Func<T, bool>>? filter,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        string includeStringProperties, bool isTracking)
    {
        IQueryable<T> query = Context.Set<T>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeStringProperties))
        {
            foreach (var includeProperty in includeStringProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync().ConfigureAwait(false);
        }

        return (!isTracking) ? await query.AsNoTracking().ToListAsync() : await query.ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity), "Entity can not be null");
        await _dataset.AddAsync(entity);        
        return entity;
    }

    public void Delete(T entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity), "Entity can not be null");
        _dataset.Remove(entity);        
    }

    public async Task<T> GetOneAsync(Guid id)
    {
        return await _dataset.FindAsync(id) ?? throw new ArgumentNullException(nameof(id));

    }

    public T Update(T entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity), "Entity can not be null");
        _dataset.Update(entity);
        return entity;
    }

}
