using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BeachBuddy.Entities;
using BeachBuddy.Enums;
using BeachBuddy.Models;
using BeachBuddy.Models.Dtos.Score;
using BeachBuddy.Repositories;
using BeachBuddy.Services.Notification;
using Microsoft.AspNetCore.Mvc;

namespace BeachBuddy.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ScoresController : ControllerBase
    {
        private readonly IBeachBuddyRepository _beachBuddyRepository;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public ScoresController(IBeachBuddyRepository beachBuddyRepository, 
            INotificationService notificationService,
            IMapper mapper)
        {
            _beachBuddyRepository = beachBuddyRepository ??
                                    throw new ArgumentNullException(nameof(beachBuddyRepository));
            _notificationService = notificationService;
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
        }
        
        [HttpGet("scores")]
        public async Task<ActionResult<IEnumerable<Score>>> GetScores()
        {
            var scoresFromRepo = await _beachBuddyRepository.GetScores();
            return Ok(_mapper.Map<IEnumerable<ScoreDto>>(scoresFromRepo));
        }

        [HttpPost("updateScore/{scoreId}")]
        public async Task<ActionResult> UpdateScore(Guid scoreId, UpdateScoreDto updateScoreDto)
        {
            var scoreToUpdate = await _beachBuddyRepository.GetScore(scoreId);
            if (scoreToUpdate == null)
            {
                return NotFound();
            }

            if (updateScoreDto.WinCount < 0)
            {
                return BadRequest("You can't be that bad! Win count must be greater than 0.");
            }
            
            _mapper.Map(updateScoreDto, scoreToUpdate);
            _beachBuddyRepository.UpdateScore(scoreToUpdate);
            await _beachBuddyRepository.Save();

            await _notificationService.sendNotification(null, NotificationType.ScoreUpdated, null, null, true);
            return NoContent();
        }
        
        [HttpPost("addScore")]
        public async Task<ActionResult> CreateScore(AddScoreDto addScoreDto)
        {
            var users = await _beachBuddyRepository.GetUsers();
            
            foreach (var user in users)
            {
                var scoreToAdd = new Score()
                {
                    Id = Guid.NewGuid(),
                    Name = addScoreDto.Name,
                    UserId = user.Id,
                    WinCount = 0,
                    User = user
                };

                user.Scores.Add(scoreToAdd);
                
                _beachBuddyRepository.UpdateUser(user);
                await _beachBuddyRepository.AddScore(scoreToAdd);
            }

            await _beachBuddyRepository.Save();

            await _notificationService.sendNotification(null, NotificationType.ScoreUpdated, null, null, true);
            return Ok();
        }
    }
}