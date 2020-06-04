using AutoMapper;

namespace BeachBuddy.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Entities.Item, Models.ItemDto>();
            
            CreateMap<Models.AddItemDto, Entities.Item>();
            
            CreateMap<Models.UpdateItemDto, Entities.Item>();
        }
    }
}