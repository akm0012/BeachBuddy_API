using AutoMapper;
using BeachBuddy.Models.Dtos.User;

namespace BeachBuddy.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Entities.User, UserDto>()
                .ForMember(
                    dest => dest.FullName,
                    memberOptions
                        => memberOptions.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<UpdateUserDto, Entities.User>();
        }
    }
}