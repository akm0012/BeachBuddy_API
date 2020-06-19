using AutoMapper;
using BeachBuddy.Models.Item;

namespace BeachBuddy.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Entities.Item, Models.ItemDto>();
            
            CreateMap<AddItemDto, Entities.Item>();
            
            CreateMap<Models.UpdateItemDto, Entities.Item>();
        }
    }
}