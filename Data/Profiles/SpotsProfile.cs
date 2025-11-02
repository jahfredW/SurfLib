using AutoMapper;
using SurfLib.Data.Dtos;
using SurfLib.Data.Models;

namespace SurfLib.Data.Profiles
{
    public class SpotsProfile : Profile
    {
        public SpotsProfile()
        {
            CreateMap<Spot, SpotsDTO>();
            CreateMap<SpotsDTO, Spot>();
        }
    }
}
