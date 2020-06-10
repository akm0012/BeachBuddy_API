using AutoMapper;

namespace BeachBuddy.Profiles
{
    public class RequestedItemProfile : Profile
    {
        public RequestedItemProfile()
        {
            CreateMap<Entities.RequestedItem, Models.RequestedItemDto>();
            
            CreateMap<Models.AddRequestedItemDto, Entities.RequestedItem>();
            
            CreateMap<Models.UpdateRequestedItemDto, Entities.RequestedItem>();
        }
    }
}