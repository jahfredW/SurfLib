using AutoMapper;
using SurfLib.Data.Dtos;
using SurfLib.Data.Models;

namespace SurfLib.Data.Profiles
{
    public class PrevisionsProfile : Profile
    {
        public PrevisionsProfile()
        {
            CreateMap<Prevision, PrevisionsDTO>();
            CreateMap<PrevisionsDTO, Prevision>();
        }
    }
}
