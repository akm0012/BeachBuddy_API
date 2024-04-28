using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BeachBuddy.Models;
using BeachBuddy.Models.Dtos;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BeachBuddy.Services.Weather
{
    public class OpenWeatherMapService : IWeatherService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<OpenWeatherMapService> _logger;

        public OpenWeatherMapService(ILogger<OpenWeatherMapService> logger, IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            _logger = logger;
            _clientFactory = httpClientFactory;
            _memoryCache = memoryCache;
        }

        public async Task<OpenWeatherDto> GetWeather(LatLonParameters latLonParameters)
        {
            var lat = latLonParameters.Lat;
            var lon = latLonParameters.Lon;

            var requestUri =
                $"https://api.openweathermap.org/data/3.0/onecall?lat={lat}&lon={lon}&appid={APIKeys.OpenWeatherApiKey}&units=imperial";

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

        public async Task<BeachConditionsDto> GetBeachConditions()
        {
            // Look for cached version
            if (_memoryCache.TryGetValue("GetBeachConditions", out BeachConditionsDto cachedBeachConditionsDto))
            {
                //  We found it in cache, use this.
                return cachedBeachConditionsDto;
            }

            const string requestUri = "https://api.visitbeaches.org/graphql";
            
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            const string requestBody = "{\n\t\"query\": \"query GetBeach($id: ID!) {\\n  beach(id: $id) {\\n    ...WithLastThreeReports\\n  }\\n}\\n\\nfragment WithLastThreeReports on Beach {\\n  lastThreeDaysOfReports {\\n    ...BeachReport\\n  }\\n}\\n\\nfragment BeachReport on Report {\\n  id\\n  createdAt\\n  latitude\\n  longitude\\n  beachReport {\\n    parameterCategory {\\n      ...ParameterCategory\\n    }\\n    reportParameters {\\n      parameterValues {\\n        ...ParameterValue\\n      }\\n      value\\n    }\\n  }\\n}\\n\\nfragment ParameterCategory on ParameterCategory {\\n  id\\n  name\\n  description\\n}\\n\\nfragment ParameterValue on ParameterValue {\\n  id\\n  name\\n  description\\n  value\\n}\\n\",\n\t\"variables\": {\n\t\t\"id\": \"2\"\n\t}\n}";
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            
            var client = _clientFactory.CreateClient();
            
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;
            await using var responseStream = await response.Content.ReadAsStreamAsync();

            var visitBeachesDto = await JsonSerializer.DeserializeAsync<VisitBeachesDto>(responseStream, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            });
            
            // Strip out the info we really care about 
            BeachConditionsDto beachConditionsDto;
            try
            {
                var beachReport = visitBeachesDto.Data.Beach.LastThreeDaysOfReports[0];
                
                // Beach Report Indexes
                const int FLAG_COLOR_INDEX = 0; 
                const int RESPIRATORY_IRRITATION_INDEX = 7; 
                const int SURF_HEIGHT_INDEX = 2; 
                const int SURF_CONDITION_INDEX = 2; 
                const int JELLY_FISH_INDEX = 8; 
                
                // Old versions that worked for like 30 min 
                // const int FLAG_COLOR_INDEX = 0; 
                // const int RESPIRATORY_IRRITATION_INDEX = 5; 
                // const int SURF_HEIGHT_INDEX = 1; 
                // const int SURF_CONDITION_INDEX = 1; 
                // const int JELLY_FISH_INDEX = 7; 
                
                var updatedTime = beachReport.CreatedAt;
                var flagColor = beachReport.BeachReport[FLAG_COLOR_INDEX].ReportParameters[0].ParameterValues[0].Name;
                var respiratoryIrritation = beachReport.BeachReport[RESPIRATORY_IRRITATION_INDEX].ReportParameters[0].ParameterValues[0].Name;
                
                var surfHeight = beachReport.BeachReport[SURF_HEIGHT_INDEX].ReportParameters[2].ParameterValues[0].Name;
                var surfCondition = beachReport.BeachReport[SURF_CONDITION_INDEX].ReportParameters[3].ParameterValues[0].Name;

                var jellyFish = beachReport.BeachReport[JELLY_FISH_INDEX].ReportParameters[0].ParameterValues[0].Name;

                beachConditionsDto = new BeachConditionsDto()
                {
                    UpdatedTime = updatedTime,
                    FlagColor = flagColor,
                    RespiratoryIrritation = respiratoryIrritation,
                    SurfHeight = surfHeight,
                    SurfCondition = surfCondition,
                    JellyFishPresent = jellyFish,
                };

            }
            catch (Exception e)
            {
                // Something messed up! 
                _logger.LogError(e, e.InnerException?.Message);
                return null;
            }

            // Save to cache
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetAbsoluteExpiration(TimeSpan
                    .FromMinutes(15));
            // Save data in cache.
            _memoryCache.Set("GetBeachConditions", beachConditionsDto, cacheEntryOptions);
            
            return beachConditionsDto;
        }

        /**
         * This was the old way where I stripped some HTML. Going to keep this as it may come in handy later. 
         */
        public async Task<VisitBeachesDto> GetBeachConditionsOld()
        {
            // Look for cached version
            if (_memoryCache.TryGetValue("GetBeachConditions", out VisitBeachesDto visitBeachesDto))
            {
                //  We found it in cache, use this.
                return visitBeachesDto;
            }
            
            // Note that this does not work anymore. The visitbeaches.org website now uses a GraphQL endpoint to get all this info.
            
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