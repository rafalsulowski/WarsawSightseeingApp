using ProjektZespolowy.DataAccess.IRepository;
using ProjektZespolowy.DataAccess.Repository;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Services.CommentPlaceService
{
    public class CommentPlaceService : ICommentPlaceService
    {
        private readonly ICommentPlaceRepository _CommentPlaceRepository;
        public CommentPlaceService(ICommentPlaceRepository CommentPlaceRepository)
        {
            _CommentPlaceRepository = CommentPlaceRepository;
        }

        public async Task<RepositoryResponse<bool>> CreateCommentPlace(CommentPlace CommentPlace)
        {
            _CommentPlaceRepository.Add(CommentPlace);
            var response = await _CommentPlaceRepository.SaveChangesAsync();
            return response;
        }

        public async Task<RepositoryResponse<bool>> LikeComment(CommentPlaceLikes like)
        {
            _CommentPlaceRepository.LikeComment(like);
            return await _CommentPlaceRepository.SaveChangesAsync();
        }

        public async Task<RepositoryResponse<bool>> DeleteCommentPlace(CommentPlace CommentPlace)
        {
            _CommentPlaceRepository.Remove(CommentPlace);
            var response = await _CommentPlaceRepository.SaveChangesAsync();
            return response;
        }

        public async Task<RepositoryResponse<CommentPlace>> GetCommentPlaceAsync(Expression<Func<CommentPlace, bool>> filter, string? includeProperties = null)
        {
            var response = await _CommentPlaceRepository.GetFirstOrDefault(filter, includeProperties);
            return response;
        }

        public async Task<RepositoryResponse<List<CommentPlace>>> GetCommentPlacesAsync(Expression<Func<CommentPlace, bool>>? filter = null, string? includeProperties = null)
        {
            var response = await _CommentPlaceRepository.GetAll(filter, includeProperties);
            return response;
        }

        public async Task<RepositoryResponse<bool>> UpdateCommentPlace(CommentPlace CommentPlace)
        {
            var response = await _CommentPlaceRepository.Update(CommentPlace);
            if (response.Success == false)
            {
                return response;
            }
            response = await _CommentPlaceRepository.SaveChangesAsync();
            return response;
        }
    }
}
