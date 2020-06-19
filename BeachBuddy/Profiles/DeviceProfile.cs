using AutoMapper;
using BeachBuddy.Entities;

namespace BeachBuddy.Profiles
{
    public class DeviceProfile : Profile
    {

        public DeviceProfile()
        {
            CreateMap<Models.AddDeviceDto, Entities.Device>();
        }
    }
}