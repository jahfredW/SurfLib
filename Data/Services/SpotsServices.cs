using SurfLib.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace SurfLib.Data.Services
{
    public class SpotsServices
    {
        private readonly SurfDbContext _context;

        public SpotsServices(SurfDbContext context)
        {
            _context = context;
        }

        // Ajouter un spot de surf
        public void AddSpot(Spot spot)
        {
            if (spot == null) throw new ArgumentNullException(nameof(spot));

            _context.Spots.Add(spot);
            _context.SaveChanges();
        }

        // Supprimer un spot de surf
        public void DeleteSpot(Spot spot)
        {
            if ( spot == null) throw new ArgumentNullException(nameof(spot));

            _context.Spots.Remove(spot);
            _context.SaveChanges();
        }

        // Lire un spot
        public Spot? GetSpot(int id) 
        {
            return _context.Spots.FirstOrDefault(spot => spot.SpotId == id);
        }

        // Lire un spot par le nom
        public Spot? GetSpotByName(string name)
        {

            return _context?.Spots.FirstOrDefault(s => s.SpotName.ToUpper() == name.ToUpper());
        }

        // Get All Spot
        public IEnumerable<Spot> GetAllSpots()
        {
            return _context.Spots.ToList();
        }

        // Update un spot
        public void SpotUpdate(Spot spot)
        {
            // test si spot null 
            if (spot == null) throw new ArgumentNullException(nameof(spot));

            // récupération du spot à modifier
            Spot? spotToUpdate = _context.Spots.FirstOrDefault(s => s.SpotId == spot.SpotId);

            if(spotToUpdate != null)
            {
                spotToUpdate.SpotName = spot.SpotName;
                spotToUpdate.SpotLat = spot.SpotLat;
                spotToUpdate.SpotLon = spot.SpotLon;

                _context.Update(spotToUpdate);
                _context.SaveChanges();
            }
        }

        // si on veut mettre à jour prévisions, il faut le faire dans la classe prévision. 
    }
}
