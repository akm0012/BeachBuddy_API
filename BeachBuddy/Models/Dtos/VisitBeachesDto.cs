using Newtonsoft.Json;

namespace BeachBuddy.Models.Dtos
{
    public class VisitBeachesDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Latest Latest { get; set; }
    }

    public class Latest
    {
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
        
        public string Flag { get; set; }
        
        public string Deadfish { get; set; }

        [JsonProperty("water_color")]
        public string WaterColor { get; set; }
        
        public string Surf { get; set; }
        
        [JsonProperty("surf_type")]
        public string SurfType { get; set; }
        
        [JsonProperty("surf_height")]
        public string SurfHeight { get; set; }
        
        [JsonProperty("respiratory_irritation")]
        public string RespiratoryIrritation { get; set; }
        
        [JsonProperty("water_surface_tempature")]
        public string WaterTemp { get; set; }
        
        [JsonProperty("air_tempature")]
        public string AirTemp { get; set; }
        
        [JsonProperty("crowds")]
        public string Crowds { get; set; }
        
        [JsonProperty("jellyfish")]
        public string JellyFish { get; set; }
        
        [JsonProperty("rip_current")]
        public string RipCurrent { get; set; }
        
        [JsonProperty("weather_summary")]
        public string WeatherSummary { get; set; }
        
        [JsonProperty("wind_speed")]
        public string WindSpeed { get; set; }
        
        [JsonProperty("red_drift")]
        public string RedDrift { get; set; }
    }
}