using SurfLib.Data.Models;
using System.Security.Cryptography;

namespace SurfLib.Data.Repositories
{
    public interface ISpotRepository
    {
        void AddSpot(Spot spot);
        Spot? GetSpotByName(string name);
        IEnumerable<Spot> GetAllSpots();
        Task SpotUpdateAsync(Spot spot);


    }
}
