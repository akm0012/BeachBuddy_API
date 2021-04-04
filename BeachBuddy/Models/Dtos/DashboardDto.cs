using System.Collections.Generic;
using BeachBuddy.Models.Dtos.Item;
using BeachBuddy.Models.Dtos.User;

namespace BeachBuddy.Models.Dtos
{
    public class DashboardDto
    {
        public IEnumerable<UserDto> Users { get; set; }
        
        public BeachConditionsDto BeachConditions { get; set; }
        
        public DashboardUVDto DashboardUvDto { get; set; }
        
        public OpenWeatherDto WeatherInfo { get; set; }
    }
}