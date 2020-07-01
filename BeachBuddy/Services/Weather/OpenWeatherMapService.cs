using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BeachBuddy.Models;
using BeachBuddy.Models.Dtos;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BeachBuddy.Services.Weather
{
    public class OpenWeatherMapService : IWeatherService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMemoryCache _memoryCache;

        public OpenWeatherMapService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            _clientFactory = httpClientFactory;
            _memoryCache = memoryCache;
        }

        public async Task<OpenWeatherDto> GetWeather(LatLonParameters latLonParameters)
        {
            var lat = latLonParameters.Lat;
            var lon = latLonParameters.Lon;

            var requestUri =
                $"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lon}&appid={APIKeys.OpenWeatherApiKey}&units=imperial";

            // Look for cached version
            if (_memoryCache.TryGetValue(requestUri, out OpenWeatherDto weatherDto))
            {
                //  We found it in cache, use this.
                return weatherDto;
            }
            
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            // request.Headers.Add("Accept", "application/vnd.github.v3+json");
            // request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode) return null;
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            weatherDto = await JsonSerializer.DeserializeAsync<OpenWeatherDto>(responseStream,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
            
            // Save to cache
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetAbsoluteExpiration(TimeSpan
                    .FromMinutes(1));
            // Save data in cache.
            _memoryCache.Set(requestUri, weatherDto, cacheEntryOptions);
            
            return weatherDto;
        }

        public async Task<OpenUVDto> GetCurrentUVIndex(LatLonParameters latLngParameters)
        {
            var lat = latLngParameters.Lat;
            var lon = latLngParameters.Lon;

            var requestUri = $"https://api.openuv.io/api/v1/uv?lat={lat}&lng={lon}";

            // Look for cached version
            if (_memoryCache.TryGetValue(requestUri, out OpenUVDto openUvDto))
            {
                //  We found it in cache, use this.
                return openUvDto;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("x-access-token", APIKeys.OpenUVIndexApiKey);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;
            await using var responseStream = await response.Content.ReadAsStreamAsync();

            openUvDto = await JsonSerializer.DeserializeAsync<OpenUVDto>(responseStream, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            });

            // Save to cache
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetAbsoluteExpiration(TimeSpan
                    .FromMinutes(
                        30)); // We are allowed 50 hits per day. We will only ever hit this 48 times. (2 * 24 = 48) 
            // Save data in cache.
            _memoryCache.Set(requestUri, openUvDto, cacheEntryOptions);

            return openUvDto;
        }

        public async Task<VisitBeachesDto> GetBeachConditions()
        {
            // Look for cached version
            if (_memoryCache.TryGetValue("GetBeachConditions", out VisitBeachesDto visitBeachesDto))
            {
                //  We found it in cache, use this.
                return visitBeachesDto;
            }
            
            // Scrape the HTML from VisitBeaches to get the beach conditions. 
            List<VisitBeachesDto> visitBeachesDtos;
            WebClient client = new WebClient();
            var htmlCode = await client.DownloadStringTaskAsync("https://visitbeaches.org/#");
            var startingIndex = htmlCode.IndexOf("var beaches = [") + 14;
            var firstSplit = htmlCode.Substring(startingIndex);
            var endIndex = firstSplit.IndexOf("];") + 1;
            var finalString = htmlCode.Substring(startingIndex, endIndex);
            visitBeachesDtos = JsonConvert.DeserializeObject<List<VisitBeachesDto>>(finalString);

            visitBeachesDto = visitBeachesDtos[1];

            // Save to cache
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetAbsoluteExpiration(TimeSpan
                    .FromMinutes(15));
            // Save data in cache.
            _memoryCache.Set("GetBeachConditions", visitBeachesDto, cacheEntryOptions);
            
            return visitBeachesDto;
        }
    }
}