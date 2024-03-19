using ProjektZespolowy.Models;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.DataAccess.IRepository
{
    public interface IPlaceRepository : IRepository<Place>
    {
        Task<RepositoryResponse<bool>> Update(Place place);
        void LikePlace(PlaceLikes like);
        void DeletePlaceLike(int userId, int PlaceId);
    }
}
