using System.Threading.Tasks;
using BeachBuddy.Models;

namespace BeachBuddy.Services.Weather
{
    public interface IWeatherService
    {
        Task<OpenWeatherDto> GetWeather(string lat, string lon);
    }
}