using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Core.Base;
using TimeTracking.Core.Specifications;

namespace TimeTracking.Core.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        string includeString = null,
                                        bool disableTracking = true);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        List<Expression<Func<T, object>>> includes = null,
                                        bool disableTracking = true);
        Task<IReadOnlyList<T>> GetAsync(ISpecification<T> spec);
        Task<T?> GetByIdAsync(Guid id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate,
                                List<Expression<Func<T, object>>> includes = null,
                                bool disableTracking = true);
        Task<T> AddAsync(T entity);
        Task<int> AddRangeAsync(T[] entity);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(T[] entities);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(T[] entities);
        Task<int> CountAsync(ISpecification<T> spec);
    }
}
