using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MogoDBClient.Contract.Repositories
{
    public interface _IRepository<T>
    {
        IQueryable<T> GetAll();

        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T obj);

        Task<T> UpdateAsync(T obj);

        Task DeleteAsync(Expression<Func<T, bool>> predicate);
    }
}
