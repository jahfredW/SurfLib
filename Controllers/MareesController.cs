using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SurfLib.Data.Dtos;
using SurfLib.Data.Models;
using SurfLib.Data.Services;
using SurfLib.Utils;

namespace SurfLib.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MareesController : ControllerBase
    {
        private readonly MareeScrapper _mareeScrapper;
        private readonly SpotsServices _spotsService;
        private readonly MareesService _mareesService;
        private readonly CityService _cityService;
        private readonly IMapper _mapper;

        public MareesController(
            SpotsServices spotsService,
            IMapper mapper,
            CityService cityService,
            MareeScrapper mareeScrapper,
            MareesService mareesService)
        {
            _spotsService = spotsService;
            _mareesService = mareesService;
            _cityService = cityService;
            _mareeScrapper = mareeScrapper;
            _mapper = mapper;
        }

        [HttpGet("{cityName}", Name = nameof(GetMareesBySpotName))]
        public async Task<IActionResult> GetMareesBySpotName(string cityName)
        {
            DateTime today = DateTime.Today;
            DateTime purgeDate = today.AddDays(-5);

            // 1️⃣ Vérification si le spot est en base
            Spot? spot = _spotsService.GetSpotByName(cityName);

            if (spot == null)
            {
                // Utilisation de cityService pour rechercher le spot
                CityInfoDTO? cityInfoDto = await _cityService.GetCityInfoAsync(cityName);
                if (cityInfoDto == null)
                    return NotFound($"Aucune information trouvée pour {cityName}.");

                // Création du spot
                spot = new Spot
                {
                    SpotName = cityInfoDto.CityName,
                    SpotLat = cityInfoDto.Latitude,
                    SpotLon = cityInfoDto.Longitude,
                    CreatedAt = cityInfoDto.CreatedAt,
                    UpdatedAt = null
                };

                _spotsService.AddSpot(spot);
            }

            // 2️⃣ Récupération des marées
            var mareesList = spot.Marees?.ToList() ?? new List<Maree>();

            bool hasMarees = mareesList.Any();
            bool isFresh = spot.UpdatedAt.HasValue && spot.UpdatedAt.Value > purgeDate;

            if (hasMarees && isFresh)
            {
                return Ok(_mapper.Map<IEnumerable<MareeDTO>>(mareesList));
            }

            // 3️⃣ Suppression des anciennes marées
            if (hasMarees)
            {
                await _mareesService.DeleteMarees(mareesList);
            }

            // 4️⃣ Scrapping des nouvelles marées
            IEnumerable<Maree> mareeList = await _mareeScrapper.GetDocument(spot);

            // ⚠️ Ne jamais mettre le Spot directement dans les marées pour éviter la boucle
            var mareesDTO = _mapper.Map<IEnumerable<MareeDTO>>(mareeList);
            // Attentin ! spot.Marees = marées qui font références au spot -> références circulaires ! 
            //var mareesDTO = _mapper.Map<IEnumerable<MareeDTO>>(spot.Marees);

            //foreach (var maree in mareeList)
            //{
            //    maree.SpotId = spot.SpotId;
            //}

            spot.UpdatedAt = DateTime.UtcNow;
            spot.Marees = mareeList.ToList();

            await _spotsService.SpotUpdateAsync(spot);

            // Mapping en DTO pour évites les références circulaires (Marées - Spot)
            

            return Ok(mareesDTO);
        }
    }
}

