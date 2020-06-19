using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BeachBuddy.Entities;
using BeachBuddy.Models;
using BeachBuddy.Models.Dtos.RequestedItem;
using BeachBuddy.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BeachBuddy.Controllers
{
    [ApiController]
    [Route("api/requestedItems")]
    public class RequestedItemController : ControllerBase
    {
        private readonly IBeachBuddyRepository _beachBuddyRepository;
        private readonly IMapper _mapper;

        public RequestedItemController(IBeachBuddyRepository beachBuddyRepository, IMapper mapper)
        {
            _beachBuddyRepository = beachBuddyRepository ??
                                    throw new ArgumentNullException(nameof(beachBuddyRepository));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestedItemDto>>> GetRequestedItems()
        {
            return Ok(_mapper.Map<IEnumerable<RequestedItemDto>>(await _beachBuddyRepository.GetRequestedItems()));
        }
        
        [HttpGet("notCompleted")]
        public async Task<ActionResult<IEnumerable<RequestedItemDto>>> GetNotCompletedRequestedItems()
        {
            return Ok(_mapper.Map<IEnumerable<RequestedItemDto>>(await _beachBuddyRepository.GetNotCompletedRequestedItems()));
        }
        
        [HttpPost]
        public async Task<ActionResult> CreateRequestedItem(AddRequestedItemDto requestedItemDto)
        {
            var requestedByUserId = requestedItemDto.RequestedByUserId;
            if (requestedByUserId != Guid.Empty && await _beachBuddyRepository.UserExists(requestedByUserId) == false)
            {
                return NotFound("User was not found.");
            }
            
            var itemToAdd = _mapper.Map<Entities.RequestedItem>(requestedItemDto);
            itemToAdd.CreatedDateTime = DateTimeOffset.UtcNow;
            itemToAdd.CompletedDateTime = null;
            
            await _beachBuddyRepository.AddRequestedItem(itemToAdd);
            await _beachBuddyRepository.Save();

            var itemToReturn = _mapper.Map<RequestedItemDto>(itemToAdd);

            return Ok(itemToReturn);
        }
        
        [HttpPost("{requestedItemId}")]
        public async Task<IActionResult> UpdateRequestedItem(Guid requestedItemId, UpdateRequestedItemDto updateItemDto)
        {
            var itemToUpdate = await _beachBuddyRepository.GetRequestedItem(requestedItemId);
            if (itemToUpdate == null)
            {
                return NotFound();
            }

            if (updateItemDto.Name == null)
            {
                updateItemDto.Name = itemToUpdate.Name;
            }

            if (updateItemDto.Count == 0)
            {
                updateItemDto.Count = itemToUpdate.Count;
            }

            // Only set the completed time if it was previously not set
            if (updateItemDto.IsRequestCompleted && !itemToUpdate.IsRequestCompleted)
            {
                itemToUpdate.CompletedDateTime = DateTimeOffset.UtcNow;
            }
            
            _mapper.Map(updateItemDto, itemToUpdate);
            _beachBuddyRepository.UpdateRequestedItem(itemToUpdate);
            await _beachBuddyRepository.Save();

            var itemToReturn = _mapper.Map<RequestedItemDto>(itemToUpdate);

            return Ok(itemToReturn);
        }
        
        [HttpDelete("{requestedItemId}")]
        public async Task<IActionResult> DeleteItem(Guid requestedItemId)
        {
            var itemToDelete = await _beachBuddyRepository.GetRequestedItem(requestedItemId);
            if (itemToDelete == null)
            {
                return NotFound();
            }

            _beachBuddyRepository.DeleteRequestedItem(itemToDelete);
            await _beachBuddyRepository.Save();

            return NoContent();
        }
    }
}