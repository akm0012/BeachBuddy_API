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
        public async Task<ActionResult<VisitBeachesDto>> GetWeather()
        {
            return Ok(await _weatherService.GetBeachConditions());
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