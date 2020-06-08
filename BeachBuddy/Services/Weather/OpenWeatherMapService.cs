using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BeachBuddy.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
            var requestUri = $"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lon}&appid={APIKeys.OpenWeatherApiKey}&units=imperial";

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

        public async Task<VisitBeachesDto> GetBeachConditions()
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

            var visitBeachesDto = visitBeachesDtos[1];
            
            return visitBeachesDto;
        }
    }
}