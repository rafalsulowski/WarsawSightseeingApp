using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ProjektZespolowy.DataAccess;
using ProjektZespolowy.DataAccess.Data;
using ProjektZespolowy.DataAccess.IRepository;
using ProjektZespolowy.Models;

namespace ProjektZespolowy.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<RepositoryResponse<bool>> CreateUser(User user)
        {
            _userRepository.Add(user);
            var response = await _userRepository.SaveChangesAsync();
            return response;
        }

        public async Task<RepositoryResponse<bool>> DeleteUser(User user)
        {
            _userRepository.Remove(user);
            var response = await _userRepository.SaveChangesAsync();
            return response;
        }

        public async Task<RepositoryResponse<User>> GetUserAsync(Expression<Func<User, bool>> filter, string? includeProperties = null)
        {
            var response = await _userRepository.GetFirstOrDefault(filter, includeProperties);
            return response;
        }

        public async Task<RepositoryResponse<List<User>>> GetUsersAsync(Expression<Func<User, bool>>? filter = null, string? includeProperties = null)
        {
            var response = await _userRepository.GetAll(filter, includeProperties);
            return response;
        }

        public async Task<RepositoryResponse<bool>> UpdateUser(User user)
        {
            var response = await _userRepository.Update(user);
            if(response.Success==false)
            {
                return response;
            }
            response = await _userRepository.SaveChangesAsync();
            return response;
        }
    }
}
