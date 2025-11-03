using Microsoft.EntityFrameworkCore;
using SurfLib.Data.Models;

namespace SurfLib.Data.Services
{
    public class MareesService
    {
        private readonly SurfDbContext _context;

        public MareesService(SurfDbContext context)
        {
            _context = context;
        }

        // Ajouter une maree
        public void AddMaree(Maree maree)
        {
            if (maree == null) throw new ArgumentNullException(nameof(maree));

            _context.Marees.Add(maree);
            _context.SaveChanges();
        }

        // Supprimer une maree
        public void DeleteMaree(Maree maree)
        {
            if (maree == null) throw new ArgumentNullException(nameof(maree));

            _context.Marees.Remove(maree);
            _context.SaveChanges();
        }

        // Suppriemr une liste de marées 
        public async Task DeleteMarees(IEnumerable<Maree> marees)
        {
            if( marees == null || !marees.Any())
            {
                throw new ArgumentNullException(nameof(marees));
            }

            _context.Marees.RemoveRange(marees);
            await _context.SaveChangesAsync();
        }

        // Lire une marée
        public Maree? GetMaree(int id)
        {
            return _context.Marees.FirstOrDefault(maree => maree.MareeId == id);
        }

        // Lire une marée par son nom de spot
        public Maree? GetMareeByName(string name)
        {

            return _context?.Marees.
                Include(m => m.Spot). // chargement de la relaation via la propriété de navigation
                FirstOrDefault(s => s.Spot.SpotName.ToUpper() == name.ToUpper());
        }

        // Get All Marees
        public IEnumerable<Maree> GetAllMarees()
        {
            return _context.Marees.ToList();
        }

        // Update une Maree
        public async Task MareeUpdateAsync(Maree maree)
        {
            // test si spot null 
            if (maree == null) throw new ArgumentNullException(nameof(maree));

            // récupération du spot à modifier
            Maree? mareeToUpdate = _context.Marees.FirstOrDefault(m => m.MareeId == maree.MareeId);

            if (mareeToUpdate != null)
            {
                mareeToUpdate.MareeHeure = maree.MareeHeure;
                mareeToUpdate.MareeDate = maree.MareeDate;
                mareeToUpdate.MareeHauteur = maree.MareeHauteur;
                mareeToUpdate.MareeCoefficient = maree.MareeCoefficient;
                mareeToUpdate.MareeMoment = maree.MareeMoment;

                // Pas besoin de update car l'entité est déja suivie par 'context', elle a déja été récupérée. 
                //_context.Update(spotToUpdate);
                await _context.SaveChangesAsync();
            }
        }
    }
    }
