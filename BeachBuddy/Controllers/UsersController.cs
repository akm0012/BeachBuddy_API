using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BeachBuddy.Entities;
using BeachBuddy.Models;
using BeachBuddy.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BeachBuddy.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IBeachBuddyRepository _beachBuddyRepository;
        private readonly IMapper _mapper;

        public UsersController(IBeachBuddyRepository beachBuddyRepository, IMapper mapper)
        {
            _beachBuddyRepository = beachBuddyRepository ??
                                    throw new ArgumentNullException(nameof(beachBuddyRepository));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var usersFromRepo = await _beachBuddyRepository.GetUsers();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(usersFromRepo));
        }
        
        [HttpGet("{userId}", Name = "GetUser")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser(Guid userId)
        {
            var userFromRepo = await _beachBuddyRepository.GetUser(userId);
            if (userFromRepo == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<UserDto>(userFromRepo));
        }
        
        [HttpPost("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, UpdateUserDto updateUserDto)
        {
            var userToUpdate = await _beachBuddyRepository.GetUser(userId);
            if (userToUpdate == null)
            {
                return NotFound();
            }
        
            _mapper.Map(updateUserDto, userToUpdate);
            _beachBuddyRepository.UpdateUser(userToUpdate);
            await _beachBuddyRepository.Save();
        
            return NoContent();
        }
        
        [HttpPost("{userId}/incrementCount")]
        public async Task<IActionResult> IncrementCountForUser(Guid userId, IncrementCountDto incrementCountDto)
        {
            var userToUpdate = await _beachBuddyRepository.GetUser(userId);
            if (userToUpdate == null)
            {
                return NotFound();
            }
        
            switch (incrementCountDto.AttributeName)
            {
                case "StarCount":
                    userToUpdate.StarCount += incrementCountDto.IncrementAmount;
                    if (userToUpdate.StarCount < 0)
                    {
                        userToUpdate.StarCount = 0;
                    }
                    break;
                
                case "KanJamWinCount":
                    userToUpdate.KanJamWinCount += incrementCountDto.IncrementAmount;
                    if (userToUpdate.KanJamWinCount < 0)
                    {
                        userToUpdate.KanJamWinCount = 0;
                    }
                    break;
            }
        
            _beachBuddyRepository.UpdateUser(userToUpdate);
            await _beachBuddyRepository.Save();
            
            return Ok(_mapper.Map<UserDto>(userToUpdate));
        }
    }
}