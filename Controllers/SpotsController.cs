using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SurfLib.Data.Dtos;
using SurfLib.Data.Models;
using SurfLib.Data.Services;
using System.Runtime;

namespace SurfLib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotsController : ControllerBase
    {
        private readonly SpotRepository _service;
        private readonly IMapper _mapper;
        private readonly CityRepository _cityService;

        public SpotsController(SpotRepository service, IMapper mapper, CityRepository cityService)
        {
            _service = service;
            _mapper = mapper;
            _cityService = cityService;
        }

        [HttpGet]
        public IActionResult GetAllSpot()
        {
            IEnumerable<Spot> spotsList = _service.GetAllSpots();
            return Ok(_mapper.Map<IEnumerable<SpotsDTO>>(spotsList));
        }

        // Get api/spots/{cityName}
        [HttpGet("{cityName}", Name = nameof(GetSpotByName))]
        public async Task<IActionResult> GetSpotByName(string cityName)
        {
            Spot? spot = _service.GetSpotByName(cityName);

            if (spot != null)
            {
                return Ok(_mapper.Map<SpotsDTO>(spot));
            }

            // appel au service de récupération de la ville 

            var cityInfo = await _cityService.GetCityInfoAsync(cityName);

            if (cityInfo == null)
            {
                return NotFound($"Aucune information trouvée pour la ville '{cityName}'.");
            }

            var newSpot = new Spot
            {
                SpotName = cityInfo.CityName.ToLower(),
                SpotLat = cityInfo.Latitude,
                SpotLon = cityInfo.Longitude,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };


            _service.AddSpot(newSpot);

            return CreatedAtRoute(nameof(GetSpotByName), new { cityName = newSpot.SpotName }, _mapper.Map<SpotsDTO>(newSpot));
        }
    }
}
