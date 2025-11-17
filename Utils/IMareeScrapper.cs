using SurfLib.Data.Models;

namespace SurfLib.Utils
{
    public interface IMareeScrapper
    {
        Task<IEnumerable<Maree>> GetDocument(Spot spot);
    }
}
