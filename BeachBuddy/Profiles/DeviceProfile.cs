using AutoMapper;
using BeachBuddy.Entities;
using BeachBuddy.Models.Dtos.Device;

namespace BeachBuddy.Profiles
{
    public class DeviceProfile : Profile
    {

        public DeviceProfile()
        {
            CreateMap<AddDeviceDto, Entities.Device>();
        }
    }
}