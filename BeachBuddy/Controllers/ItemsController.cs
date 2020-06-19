
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BeachBuddy.Entities;
using BeachBuddy.Models;
using BeachBuddy.Models.Item;
using BeachBuddy.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BeachBuddy.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly IBeachBuddyRepository _beachBuddyRepository;
        private readonly IMapper _mapper;

        public ItemsController(IBeachBuddyRepository beachBuddyRepository, IMapper mapper)
        {
            _beachBuddyRepository = beachBuddyRepository ??
                                    throw new ArgumentNullException(nameof(beachBuddyRepository));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return Ok(_mapper.Map<IEnumerable<ItemDto>>(await _beachBuddyRepository.GetItems()));
        }
        
        [HttpGet("{itemId}", Name = "GetItem")]
        public async Task<ActionResult<Item>> GetItem(Guid itemId)
        {
            var item = await _beachBuddyRepository.GetItem(itemId);

            if (item == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<ItemDto>(item));
        }

        [HttpPost]
        public async Task<ActionResult> CreateItem(AddItemDto addItemDto)
        {
            var itemToAdd = _mapper.Map<Entities.Item>(addItemDto);
            await _beachBuddyRepository.AddItem(itemToAdd);
            await _beachBuddyRepository.Save();

            var itemToReturn = _mapper.Map<ItemDto>(itemToAdd);

            return CreatedAtRoute("GetItem", new {itemId = itemToReturn.Id}, itemToReturn);
        }
        
        [HttpPost("{itemId}")]
        public async Task<IActionResult> UpdateItem(Guid itemId, UpdateItemDto updateItemDto)
        {
            var itemToUpdate = await _beachBuddyRepository.GetItem(itemId);
            if (itemToUpdate == null)
            {
                return NotFound();
            }

            _mapper.Map(updateItemDto, itemToUpdate);
            _beachBuddyRepository.UpdateItem(itemToUpdate);
            await _beachBuddyRepository.Save();

            return NoContent();
        }
        
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> UpdateItem(Guid itemId)
        {
            var itemToDelete = await _beachBuddyRepository.GetItem(itemId);
            if (itemToDelete == null)
            {
                return NotFound();
            }

            _beachBuddyRepository.DeleteItem(itemToDelete);
            await _beachBuddyRepository.Save();

            return NoContent();
        }
    }
}