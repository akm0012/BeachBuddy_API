using AutoMapper;
using BeachBuddy.Models.Dtos;

namespace BeachBuddy.Profiles
{
    public class UvProfile : Profile
    {
        public UvProfile()
        {
            CreateMap<OpenUVDto, DashboardUVDto>()
                .ForMember(d => d.uv,
                    opt
                        => opt.MapFrom(s => s.result.uv))
                .ForMember(d => d.uv_time,
                    opt
                        => opt.MapFrom(s => s.result.uv_time))
                .ForMember(d => d.uv_max,
                    opt
                        => opt.MapFrom(s => s.result.uv_max))
                .ForMember(d => d.uv_max_time,
                    opt
                        => opt.MapFrom(s => s.result.uv_max_time))
                .ForMember(d => d.safe_exposure_time,
                    opt
                        => opt.MapFrom(s => s.result.safe_exposure_time));
        }
    }
}