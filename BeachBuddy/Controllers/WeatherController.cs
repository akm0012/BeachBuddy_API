using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeachBuddy.Models;
using BeachBuddy.Services.Weather;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BeachBuddy.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        [HttpGet("siestaKeyConditions")]
        public async Task<ActionResult<OpenWeatherDto>> GetWeather()
        {
            // Scrape the HTML from VisitBeaches to get the beach conditions. 
            List<VisitBeachesDto> visitBeachesDtos;
            WebClient client = new WebClient();
            var htmlCode = await client.DownloadStringTaskAsync("https://visitbeaches.org/#");
            var startingIndex = htmlCode.IndexOf("var beaches = [") + 14;
            var firstSplit = htmlCode.Substring(startingIndex);
            var endIndex = firstSplit.IndexOf("];") + 1;
            var finalString = htmlCode.Substring(startingIndex, endIndex);
            visitBeachesDtos = JsonConvert.DeserializeObject<List<VisitBeachesDto>>(finalString);

            return Ok(visitBeachesDtos[1]);
        }

        [HttpGet]
        public async Task<ActionResult<OpenWeatherDto>> GetWeatherForLatLong(
            [FromQuery(Name = "lat")] string lat,
            [FromQuery(Name = "lon")] string lon)
        {
            // Default to Sarasota, FL
            if (lat == null || lon == null)
            {
                lat = "27.267804";
                lon = "-82.553679";
            }
            
            return Ok(await _weatherService.GetWeather(lat, lon));
        }
    }
}