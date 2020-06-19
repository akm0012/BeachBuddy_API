

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BeachBuddy.Models;
using BeachBuddy.Models.Dtos;
using BeachBuddy.Models.Dtos.Item;
using BeachBuddy.Models.Dtos.User;
using BeachBuddy.Repositories;
using BeachBuddy.Services.Weather;
using Microsoft.AspNetCore.Mvc;

namespace BeachBuddy.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IBeachBuddyRepository _beachBuddyRepository;
        private readonly IMapper _mapper;
        private readonly IWeatherService _weatherService;

        public DashboardController(IBeachBuddyRepository beachBuddyRepository, IMapper mapper, IWeatherService weatherService)
        {
            _beachBuddyRepository = beachBuddyRepository ??
                                    throw new ArgumentNullException(nameof(beachBuddyRepository));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardDto>> GetDashboardData([FromQuery] LatLonParameters latLonParameters)
        {
            var beachConditions = await _weatherService.GetBeachConditions();
            var weatherData = await _weatherService.GetWeather(latLonParameters);
            var usersFromRepo = await _beachBuddyRepository.GetUsers();
            var itemsFromRepo = await _beachBuddyRepository.GetItems();

            var dashboardDto = new DashboardDto
            {
                BeachConditions = beachConditions,
                WeatherInfo = weatherData,
                Users = _mapper.Map<IEnumerable<UserDto>>(usersFromRepo),
                Items = _mapper.Map<IEnumerable<ItemDto>>(itemsFromRepo)
            };

            return Ok(dashboardDto);
        }
    }
}