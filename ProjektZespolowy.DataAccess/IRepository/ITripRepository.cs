using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using ProjektZespolowy.Models.JoinedTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.DataAccess.IRepository
{
    public interface ITripRepository : IRepository<Trip>
    {
        Task<RepositoryResponse<bool>> Update(Trip trip);
        Task<RepositoryResponse<bool>> AddPlaceToTrip(PlacesTrips place);
        Task<RepositoryResponse<bool>> DeletePlaceFromTrip(PlacesTrips placesTrips);
    }
}
