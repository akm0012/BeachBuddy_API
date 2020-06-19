using System.Threading.Tasks;
using BeachBuddy.Models;
using BeachBuddy.Models.Dtos;

namespace BeachBuddy.Services.Weather
{
    public interface IWeatherService
    {
        Task<OpenWeatherDto> GetWeather(LatLonParameters latLonParameters);
        Task<VisitBeachesDto> GetBeachConditions();
    }
}