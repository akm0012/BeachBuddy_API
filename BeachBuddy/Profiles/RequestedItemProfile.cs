using AutoMapper;
using BeachBuddy.Models.Dtos.RequestedItem;

namespace BeachBuddy.Profiles
{
    public class RequestedItemProfile : Profile
    {
        public RequestedItemProfile()
        {
            CreateMap<Entities.RequestedItem, RequestedItemDto>();
            
            CreateMap<AddRequestedItemDto, Entities.RequestedItem>();
            
            CreateMap<UpdateRequestedItemDto, Entities.RequestedItem>();
        }
    }
}