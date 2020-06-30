using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BeachBuddy.DbContexts;
using BeachBuddy.Models;
using BeachBuddy.Models.Dtos;
using BeachBuddy.Models.Dtos.Item;
using BeachBuddy.Models.Dtos.User;
using BeachBuddy.Repositories;
using BeachBuddy.Services.Weather;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

namespace BeachBuddy.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly BeachBuddyContext _context;
        private readonly IBeachBuddyRepository _beachBuddyRepository;
        private readonly IMapper _mapper;
        private readonly IWeatherService _weatherService;
        private readonly IHostApplicationLifetime _appLifetime;

        public DashboardController(BeachBuddyContext context, IBeachBuddyRepository beachBuddyRepository,
            IMapper mapper,
            IWeatherService weatherService,
            IHostApplicationLifetime appLifetime)
        {
            _context = context;

            _beachBuddyRepository = beachBuddyRepository ??
                                    throw new ArgumentNullException(nameof(beachBuddyRepository));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
            _weatherService = weatherService;
            _appLifetime = appLifetime;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardDto>> GetDashboardData([FromQuery] LatLonParameters latLonParameters)
        {
            var beachConditions = await _weatherService.GetBeachConditions();

            // todo: Un comment when go live
            var uvDto = await _weatherService.GetCurrentUVIndex(latLonParameters);

            var weatherData = await _weatherService.GetWeather(latLonParameters);
            // var usersFromRepo = await _beachBuddyRepository.GetUsers();
            var dashboardDto = new DashboardDto
            {
                BeachConditions = beachConditions,
                DashboardUvDto = _mapper.Map<DashboardUVDto>(uvDto),
                // Todo: this can be deleted
                // DashboardUvDto = new DashboardUVDto
                // {
                //   uv  = 8.3,
                //   uv_time = "2020-06-29T13:32:07.067Z",
                //   uv_max = 12.1,
                //   uv_max_time = "2020-06-29T16:32:07.067Z",
                //   safe_exposure_time = new SafeExposureTimeDto
                //   {
                //       st1 = 10,
                //       st2 = 20,
                //       st3 = 30,
                //       st4 = 40,
                //       st5 = 50,
                //       st6 = 60
                //   }
                // },
                WeatherInfo = weatherData,
                // Users = _mapper.Map<IEnumerable<UserDto>>(usersFromRepo),
            };

            return Ok(dashboardDto);
        }

        [HttpDelete("DeleteDatabase")]
        public async Task<ActionResult> DeleteAndResetDatabase()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.MigrateAsync();

            return Ok();
        }

        [HttpDelete("StopServer")]
        public ActionResult StopServer()
        {
            _appLifetime.StopApplication();
            return Ok();
        }
    }
}