using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BeachBuddy.DbContexts;
using BeachBuddy.Enums;
using BeachBuddy.Models;
using BeachBuddy.Models.Dtos;
using BeachBuddy.Models.Dtos.Item;
using BeachBuddy.Models.Dtos.User;
using BeachBuddy.Repositories;
using BeachBuddy.Services.Notification;
using BeachBuddy.Services.Weather;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

namespace BeachBuddy.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly BeachBuddyContext _context;
        private readonly IBeachBuddyRepository _beachBuddyRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IMapper _mapper;
        private readonly IWeatherService _weatherService;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger _logger;
        private readonly INotificationService _notificationService;

        public DashboardController(BeachBuddyContext context, 
            IBeachBuddyRepository beachBuddyRepository,
            IStatusRepository statusRepository,
            IMapper mapper,
            IWeatherService weatherService,
            INotificationService notificationService,
            IHostApplicationLifetime appLifetime,
            ILogger<DashboardController> logger)
        {
            _context = context;

            _beachBuddyRepository = beachBuddyRepository ??
                                    throw new ArgumentNullException(nameof(beachBuddyRepository));
            _statusRepository = statusRepository ??
                                throw new ArgumentNullException(nameof(statusRepository));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
            _weatherService = weatherService;
            _notificationService = notificationService;
            _appLifetime = appLifetime;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardDto>> GetDashboardData([FromQuery] LatLonParameters latLonParameters)
        {
            BeachConditionsDto beachConditions = null;
            try
            {
                beachConditions = await _weatherService.GetBeachConditions();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Beach Conditions could not be grabbed: " + e.Message);
            }

            var uvDto = await _weatherService.GetCurrentUVIndex(latLonParameters);
            var weatherData = await _weatherService.GetWeather(latLonParameters);
            var usersFromRepo = await _beachBuddyRepository.GetUsers();
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
                Users = _mapper.Map<IEnumerable<UserDto>>(usersFromRepo),
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

        [HttpPost("refresh")]
        public async Task<ActionResult> NotifySystemWideRefresh()
        {
            await _notificationService.sendNotification(null, NotificationType.DashboardPulledToRefresh, null, null, true);
            return Ok();
        }

        [HttpGet("SystemStatus")]
        public async Task<ActionResult<SystemStatusDto>> GetSystemStatus()
        {
            var systemStatus = await _statusRepository.GetSystemStatus();
            return Ok(systemStatus);
        }
    }
}