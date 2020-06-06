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
            WebClient client = new WebClient();
            var htmlCode = await client.DownloadStringTaskAsync("https://visitbeaches.org/#");
            var startingIndex = htmlCode.IndexOf("var beaches = [") + 14;
            var firstSplit = htmlCode.Substring(startingIndex);
            var endIndex = firstSplit.IndexOf("];") + 1;
            var finalString = htmlCode.Substring(startingIndex, endIndex);
            List<VisitBeachesDto> test =
                JsonConvert.DeserializeObject<List<VisitBeachesDto>>(finalString);
            
            return Ok(test[1]);
        }
        
        [HttpGet("{lat},{lon}")]
        public async Task<ActionResult<OpenWeatherDto>> GetWeatherForLatLong(string lat, string lon)
        {

            
            // return await _weatherService.GetWeather(0, 0);
            return Ok();
        }
        
    }
}