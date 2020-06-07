using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BeachBuddy.Models
{
    public class OpenWeatherDto
    {
        public double lat { get; set; }

        public double lon { get; set; }

        public Current current { get; set; }
        
        public IEnumerable<MinuteForecast> minutely { get; set; }
        
        public IEnumerable<HourlyWeatherForecast> hourly { get; set; }
    }

    public class Current
    {
        // Current Time (When this was called)         
        public long dt { get; set; }

        public long sunrise { get; set; }

        public long sunset { get; set; }

        public double temp { get; set; }

        public double feels_like { get; set; }

        public int humidity { get; set; }

        public double uvi { get; set; }

        public double clouds { get; set; }

        public double wind_speed { get; set; }

        public double wind_gust { get; set; }
        
        public IEnumerable weather { get; set; }
    }

    public class Weather
    {
        public string main { get; set; }
        
        public string description { get; set; }
        
        public string icon { get; set; }
    }

    public class MinuteForecast
    {
        public long dt { get; set; }
        
        public double precipitation { get; set; }
    }
    
    public class HourlyWeatherForecast {
        public long dt { get; set; }
        
        public double temp { get; set; }

        public double feels_like { get; set; }
        
        public double humidity { get; set; }

        public double clouds { get; set; }
        
        public double wind_speed { get; set; }

        public IEnumerable weather { get; set; }
    }
}