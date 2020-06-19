using System.Threading.Tasks;
using BeachBuddy.Enums;
using BeachBuddy.Models;
using BeachBuddy.Models.Dtos;
using BeachBuddy.Services.Weather;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
            [FromQuery] LatLonParameters latLonParameters)
        {
            return Ok(await _weatherService.GetWeather(latLonParameters));
        }

        [HttpGet("safeExposure/{skinType}/{uvIndex}/{spf}")]
        public ActionResult GetSafeExposureTime(SkinType skinType, double uvIndex, int spf)
        {
            return Ok(skinType.GetSafeExposureTime(uvIndex, spf));
        }
    }
}