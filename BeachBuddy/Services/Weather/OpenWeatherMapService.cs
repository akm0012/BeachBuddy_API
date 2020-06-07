using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BeachBuddy.Models;

namespace BeachBuddy.Services.Weather
{
    public class OpenWeatherMapService : IWeatherService
    {
        private readonly IHttpClientFactory _clientFactory;

        public OpenWeatherMapService(IHttpClientFactory httpClientFactory)
        {
            _clientFactory = httpClientFactory;
        }

        public async Task<OpenWeatherDto> GetWeather(string lat, string lon)
        {
            var requestUri = "https://api.openweathermap.org/data/2.5/onecall?lat={{lat}}&lon={{lon}}&appid=055c737018d06a4165998ae99d562ac9&units=imperial";

            requestUri = requestUri.Replace("{{lat}}", lat);
            requestUri = requestUri.Replace("{{lon}}", lon);
            
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            // request.Headers.Add("Accept", "application/vnd.github.v3+json");
            // request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode) return null;
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var weatherDto = await JsonSerializer.DeserializeAsync<OpenWeatherDto>(responseStream, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            });
            return weatherDto;

        }
    }
}