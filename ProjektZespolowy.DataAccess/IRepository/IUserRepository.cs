using ProjektZespolowy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.DataAccess.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<RepositoryResponse<bool>> Update(User user);
    }
}
