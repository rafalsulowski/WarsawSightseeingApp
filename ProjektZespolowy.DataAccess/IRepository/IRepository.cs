using ProjektZespolowy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.DataAccess.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        Task<RepositoryResponse<List<T>>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<RepositoryResponse<T>> GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        Task<RepositoryResponse<bool>> SaveChangesAsync();
    }
}
