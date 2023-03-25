using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeachBuddy.DbContexts;
using BeachBuddy.Entities;
using BeachBuddy.Models;
using BeachBuddy.Models.Dtos;
using BeachBuddy.Services.Twilio;
using BeachBuddy.Services.Weather;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BeachBuddy.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly BeachBuddyContext _context;
        private readonly IWeatherService _weatherService;
        private readonly IBeachBuddyRepository _beachBuddyRepository;
        private readonly ITwilioService _twilioService;

        public StatusRepository(BeachBuddyContext buddyContext,
            IWeatherService weatherService,
            IBeachBuddyRepository beachBuddyRepository,
            ITwilioService twilioService
        )
        {
            _context = buddyContext;
            _weatherService = weatherService;
            _beachBuddyRepository = beachBuddyRepository;
            _twilioService = twilioService;
        }

        public async Task<SystemStatus> GetSystemStatus()
        {
            var systemStatus = new SystemStatus();

            // Check the Database
            await CheckDatabase(systemStatus);

            // Beach Conditions 
            await CheckBeachConditions(systemStatus);

            // Check Weather 
            await CheckWeather(systemStatus);

            // Check Current UV Index 
            await CheckUvIndex(systemStatus);

            // Check we can access the Users
            await CheckUsers(systemStatus);

            // Get Twilio balance 
            await GetTwilioBalance(systemStatus);
            
            return systemStatus;
        }

        private async Task GetTwilioBalance(SystemStatus systemStatus)
        {
            try
            {
                var account = await _twilioService.GetAccountBalance();
                systemStatus.TwilioBalance = account.Balance;
                systemStatus.IsTwilioOk = true;
            }
            catch (Exception e)
            {
                systemStatus.ErrorMessages.Add("Twilio Service isn't accessible: " + e.Message);
                systemStatus.IsTwilioOk = false;
            }
        }

        private async Task CheckUsers(SystemStatus systemStatus)
        {
            IEnumerable<User> users = null;
            try
            {
                users = await _beachBuddyRepository.GetUsers();
                
                systemStatus.IsGetUsersOk = true;
            }
            catch (Exception e)
            {
                systemStatus.ErrorMessages.Add("Users are not accessible: " + e.Message);
                systemStatus.IsGetUsersOk = false;
            }

            if (users == null)
            {
                systemStatus.ErrorMessages.Add("User data is null.");
                systemStatus.IsGetUsersOk = false;
            }
        }
        
        private async Task CheckUvIndex(SystemStatus systemStatus)
        {
            OpenUVDto uvDto = null;
            try
            {
                uvDto = await _weatherService.GetCurrentUVIndex(new LatLonParameters
                {
                    // Sarasota baby! 
                    Lat = "27.267804",
                    Lon = "-82.553679"
                });
                systemStatus.IsCurrentUvIndexOk = true;
            }
            catch (Exception e)
            {
                systemStatus.ErrorMessages.Add("UV Index info is unavailable: " + e.Message);
                systemStatus.IsCurrentUvIndexOk = false;
            }

            if (uvDto == null)
            {
                systemStatus.ErrorMessages.Add("UV Index data is null.");
                systemStatus.IsCurrentUvIndexOk = false;
            }
        }
        
        private async Task CheckWeather(SystemStatus systemStatus)
        {
            OpenWeatherDto weatherDto = null;
            try
            {
                weatherDto = await _weatherService.GetWeather(new LatLonParameters
                {
                    // Sarasota baby! 
                    Lat = "27.267804",
                    Lon = "-82.553679"
                });
                systemStatus.IsWeatherOk = true;
            }
            catch (Exception e)
            {
                systemStatus.ErrorMessages.Add("Weather data is unavailable: " + e.Message);
                systemStatus.IsWeatherOk = false;
            }

            if (weatherDto == null)
            {
                systemStatus.ErrorMessages.Add("Weather data is null.");
                systemStatus.IsWeatherOk = false;
            }
        }

        private async Task CheckBeachConditions(SystemStatus systemStatus)
        {
            BeachConditionsDto beachConditions = null;
            try
            {
                beachConditions = await _weatherService.GetBeachConditions();
                systemStatus.IsBeachConditionsOk = true;
            }
            catch (Exception e)
            {
                systemStatus.ErrorMessages.Add("Beach Conditions are unavailable: " + e.Message);
                systemStatus.IsBeachConditionsOk = false;
            }

            if (beachConditions == null)
            {
                systemStatus.ErrorMessages.Add("Beach Conditions were null.");
                systemStatus.IsBeachConditionsOk = false;
            }
        }

        private async Task CheckDatabase(SystemStatus systemStatus)
        {
            systemStatus.IsDatabaseOk = await IsDatabaseConnected();
            if (!systemStatus.IsDatabaseOk)
            {
                systemStatus.ErrorMessages.Add(
                    "The database could not be reached. Make sure the Docker Image is running.");
            }
        }

        /**
         *  https://stackoverflow.com/questions/41935752/entity-framework-core-how-to-get-the-connection-from-the-dbcontext
         */
        private async Task<bool> IsDatabaseConnected()
        {
            await using var sqlConnection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
            try
            {
                await sqlConnection.OpenAsync();
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }
    }
}