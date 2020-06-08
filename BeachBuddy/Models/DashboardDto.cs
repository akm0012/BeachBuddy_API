using System.Collections;
using System.Collections.Generic;

namespace BeachBuddy.Models
{
    public class DashboardDto
    {
        public IEnumerable<UserDto> Users { get; set; }
        
        public IEnumerable<ItemDto> Items { get; set; }

        public VisitBeachesDto BeachConditions { get; set; }
        
        public OpenWeatherDto WeatherInfo { get; set; }
    }
}