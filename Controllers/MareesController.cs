using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SurfLib.Business;
using SurfLib.Data.Dtos;
using SurfLib.Data.Models;
using SurfLib.Data.Repositories;
using SurfLib.Data.Services;
using SurfLib.Utils;

namespace SurfLib.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MareesController : ControllerBase
    {
        private readonly IMareeScrapper _mareeScrapper;
        private readonly ISpotRepository _spotRepository;
        private readonly IMareeRepository _mareeRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IMareeService _mareeService;
        private readonly IMapper _mapper;

        public MareesController(
            ISpotRepository spotRepository,
            IMapper mapper,
            ICityRepository cityRepository,
            IMareeScrapper mareeScrapper,
            IMareeRepository mareeRepository,
            IMareeService mareeService )
        {
            _spotRepository = spotRepository;
            _mareeRepository = mareeRepository;
            _cityRepository = cityRepository;
            _mareeScrapper = mareeScrapper;
            _mareeService = mareeService;
            _mapper = mapper;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllMaree()
        {
            return Ok(await _mareeRepository.GetAllMarees());
        }


        [HttpGet("{cityName}", Name = nameof(GetMareesBySpotName))]
        public async Task<IActionResult> GetMareesBySpotName(string cityName)
        {

           List<MareeDTO> mareeList = (await (_mareeService.GetMareesBySpotNameBusinessAsync(cityName))).ToList();


            // Mapping en DTO pour évites les références circulaires (Marées - Spot)
            
            return Ok(mareeList);
        }
    }
}

