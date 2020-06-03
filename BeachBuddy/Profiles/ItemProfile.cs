using AutoMapper;

namespace BeachBuddy.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Entities.Item, Models.ItemDto>();
        }
    }
}