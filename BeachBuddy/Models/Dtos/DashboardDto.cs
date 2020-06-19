using System.Collections.Generic;
using BeachBuddy.Models.Dtos.Item;
using BeachBuddy.Models.Dtos.User;

namespace BeachBuddy.Models.Dtos
{
    public class DashboardDto
    {
        public IEnumerable<UserDto> Users { get; set; }
        
        public IEnumerable<ItemDto> Items { get; set; }

        public VisitBeachesDto BeachConditions { get; set; }
        
        public OpenWeatherDto WeatherInfo { get; set; }
    }
}