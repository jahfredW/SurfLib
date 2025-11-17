using SurfLib.Data.Dtos;

namespace SurfLib.Data.Repositories
{
    public interface ICityRepository
    {
        Task<CityInfoDTO?> GetCityInfoAsync(string cityName);
    }
}
