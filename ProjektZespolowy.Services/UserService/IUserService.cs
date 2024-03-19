using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ProjektZespolowy.Models;

namespace ProjektZespolowy.Services.UserService
{
    public interface IUserService
    {
        Task<RepositoryResponse<List<User>>> GetUsersAsync(Expression<Func<User, bool>>? filter = null, string? includeProperties = null);
        Task<RepositoryResponse<User>> GetUserAsync(Expression<Func<User, bool>> filter, string? includeProperties = null);
        Task<RepositoryResponse<bool>> CreateUser(User user);
        Task<RepositoryResponse<bool>> UpdateUser(User user);
        Task<RepositoryResponse<bool>> DeleteUser(User user);
    }
}
