using SurfLib.Data.Dtos;

namespace SurfLib.Business
{
    public interface IMareeService
    {
        Task<IEnumerable<MareeDTO>> GetMareesBySpotNameBusinessAsync(string cityName);
    }
}
