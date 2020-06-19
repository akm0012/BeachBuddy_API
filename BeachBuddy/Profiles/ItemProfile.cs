using AutoMapper;
using BeachBuddy.Models.Dtos.Item;

namespace BeachBuddy.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Entities.Item, ItemDto>();
            
            CreateMap<AddItemDto, Entities.Item>();
            
            CreateMap<UpdateItemDto, Entities.Item>();
        }
    }
}