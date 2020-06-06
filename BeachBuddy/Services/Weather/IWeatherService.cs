using System.Threading.Tasks;
using BeachBuddy.Models;

namespace BeachBuddy.Services.Weather
{
    public interface IWeatherService
    {
        Task<OpenWeatherDto> GetWeather(long lat, long lon);
    }
}