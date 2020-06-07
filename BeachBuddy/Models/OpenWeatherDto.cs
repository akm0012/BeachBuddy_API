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
        
        public IEnumerable<DailyForecast> daily { get; set; }
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

        public int clouds { get; set; }

        public double wind_speed { get; set; }

        public double wind_gust { get; set; }

        public IEnumerable<Weather> weather { get; set; }
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

    public class HourlyWeatherForecast
    {
        public long dt { get; set; }

        public double temp { get; set; }

        public double feels_like { get; set; }

        public int humidity { get; set; }

        public double clouds { get; set; }

        public double wind_speed { get; set; }

        public IEnumerable weather { get; set; }
    }

    public class DailyForecast
    {
        public long dt { get; set; }

        public long sunrise { get; set; }

        public long sunset { get; set; }

        public DailyTempForecast temp { get; set; }
        
        public FeelsLikeTempForecast feels_like { get; set; }
        
        public int humidity { get; set; }
        
        public double wind_speed { get; set; }
        
        public IEnumerable weather { get; set; }
        
        public double rain { get; set; }
        
        public double uvi { get; set; }
    }

    public class DailyTempForecast
    {
        public double day { get; set; }

        public double min { get; set; }

        public double max { get; set; }

        public double night { get; set; }

        public double eve { get; set; }

        public double morn { get; set; }
    }
    
    public class FeelsLikeTempForecast
    {
        public double day { get; set; }

        public double night { get; set; }

        public double eve { get; set; }

        public double morn { get; set; }
    }
}