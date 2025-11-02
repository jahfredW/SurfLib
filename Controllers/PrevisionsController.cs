using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SurfLib.Data.Profiles;
using SurfLib.Data.Services;

namespace SurfLib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrevisionsController : ControllerBase
    {
        private readonly PrevisionsService _service;
        private readonly IMapper _mapper;

        public PrevisionsController(PrevisionsService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

    }
}
