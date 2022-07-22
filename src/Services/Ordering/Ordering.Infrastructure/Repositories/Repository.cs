using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Common;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class Repository<T> : IAsyncRepository<T, Guid> where T : Entity
    {
        private readonly OrdersContext _dbContext;

        public Repository(OrdersContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException($"Argument {nameof(dbContext)} cannot be null.");
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>()
                                   .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>()
                                   .Where(predicate)
                                   .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAsync(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeString = null,
            bool disableTracking = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> queryable = _dbContext.Set<T>();

            if (disableTracking)
                queryable = queryable.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(includeString))
                queryable = queryable.Include(includeString);

            if (predicate is not null)
                queryable = queryable.Where(predicate);

            if (orderBy is not null)
                queryable = orderBy(queryable);

            return await queryable.ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, IList<Expression<Func<T, object>>> includes = null, bool disableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<T> queryable = _dbContext.Set<T>();

            if (disableTracking)
                queryable = queryable.AsNoTracking();

            //if (includes?.Any() ?? false)
            //    includes.ToList().ForEach(include => queryable = queryable.Include(include));
            if (includes?.Any() ?? false)
                includes.Aggregate(queryable, (current, include) => current.Include(include));

            if (predicate is not null)
                queryable = queryable.Where(predicate);

            if (orderBy is not null)
                queryable = orderBy(queryable);

            return await queryable.ToListAsync(cancellationToken);
        }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            //var getResult = await GetAsync(x => x.Id == id, cancellationToken);

            //return getResult?.FirstOrDefault();

            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
