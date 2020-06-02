using AutoMapper;

namespace BeachBuddy.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Entities.User, Models.UserDto>()
                .ForMember(
                    dest => dest.FullName,
                    memberOptions
                        => memberOptions.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }
    }
}