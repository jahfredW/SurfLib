using AutoMapper;
using SurfLib.Data.Dtos;
using SurfLib.Data.Models;
using SurfLib.Data.Repositories;
using SurfLib.Data.Services;
using SurfLib.Utils;

namespace SurfLib.Business
{
    public class MareeService : IMareeService
    {
        private readonly ISpotRepository _spotsService;
        private readonly ICityRepository   _cityRepository;
        private readonly IMareeScrapper _mareeScrapper;
        private readonly IMareeRepository _mareeRepository;
        private readonly IMapper?       _mapper;


        public MareeService(ISpotRepository spotsService, ICityRepository cityRepository, IMareeScrapper mareeScrapper, IMareeRepository mareeRepository, IMapper mapper)
        {
            _spotsService = spotsService;
            _cityRepository = cityRepository;
            _mareeScrapper = mareeScrapper;
            _mareeRepository = mareeRepository;
            _mapper = mapper;

        }

        public async Task<IEnumerable<MareeDTO>> GetMareesBySpotNameBusinessAsync(string cityName)
        {
            // 📌 Recherche ou création du Spot
            Spot? spot = _spotsService.GetSpotByName(cityName);

            if (spot == null)
            {
                CityInfoDTO? cityInfoDto = await _cityRepository.GetCityInfoAsync(cityName);

                if (cityInfoDto == null)
                    throw new ArgumentException("Le spot n'existe pas et n'a pas pu être créé");

                spot = new Spot
                {
                    SpotName = cityInfoDto.CityName,
                    SpotLat = cityInfoDto.Latitude,
                    SpotLon = cityInfoDto.Longitude,
                    CreatedAt = DateTime.UtcNow
                };

                _spotsService.AddSpot(spot);
            }

            // 📌 Marées actuelles
            var mareesList = spot.Marees?.ToList() ?? new List<Maree>();

            bool hasBad = mareesList.Any(m =>
                m.MareeHeure == TimeOnly.MinValue || m.MareeHauteur == 0
            );

            bool hasGood = mareesList.Any() && !hasBad;

            bool isFresh = spot.UpdatedAt.HasValue &&
                           spot.UpdatedAt.Value.Date == DateTime.UtcNow.Date;

            // 📌 Retour immédiat si data propre + fraîche
            if (hasGood && isFresh)
                return _mapper.Map<IEnumerable<MareeDTO>>(mareesList);

            // 📌 Purge si données existantes mais incomplètes
            if (mareesList.Any())
                await _mareeRepository.DeleteMarees(mareesList);

            // 📌 Scrapping
            List<Maree> freshMarees = (await _mareeScrapper.GetDocument(spot)).ToList();

            // 📌 Affectation propre
            foreach (var m in freshMarees)
                m.SpotId = spot.SpotId;

            // 📌 Replace marées + mise à jour spot
            spot.Marees = freshMarees;
            spot.UpdatedAt = DateTime.UtcNow;

            await _spotsService.SpotUpdateAsync(spot);

            return _mapper.Map<IEnumerable<MareeDTO>>(freshMarees);
        }
    }
}
