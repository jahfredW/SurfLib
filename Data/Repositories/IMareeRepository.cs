using SurfLib.Data.Models;

namespace SurfLib.Data.Repositories
{
    public interface IMareeRepository
    {
        void AddMaree(Maree maree);
        void DeleteMaree(Maree maree);

        Task DeleteMarees(IEnumerable<Maree> marees);
        Maree? GetMaree(int id);
        Maree? GetMareeByName(string name);
        Task<List<Maree>> GetAllMarees();
        Task MareeUpdateAsync(Maree maree);

    }
}
