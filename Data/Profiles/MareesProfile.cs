using AutoMapper;
using SurfLib.Data.Dtos;
using SurfLib.Data.Models;

namespace SurfLib.Data.Profiles
{
    public class MareeProfile : Profile
    {
        public MareeProfile()
        {
            CreateMap<Maree, MareeDTO>();
        }
    }
}
