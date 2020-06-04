
using System;
using System.Collections.Generic;
using AutoMapper;
using BeachBuddy.Entities;
using BeachBuddy.Models;
using BeachBuddy.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeachBuddy.Controllers
{
    [ApiController]
    [Route("api/items", Name = "GetItem")]
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
        public ActionResult<IEnumerable<Item>> GetItems()
        {
            return Ok(_mapper.Map<IEnumerable<ItemDto>>(_beachBuddyRepository.GetItems()));
        }

        [HttpPost]
        public ActionResult CreateItem(AddItemDto addItemDto)
        {
            var itemToAdd = _mapper.Map<Entities.Item>(addItemDto);
            _beachBuddyRepository.AddItem(itemToAdd);
            _beachBuddyRepository.Save();

            var itemToReturn = _mapper.Map<ItemDto>(itemToAdd);

            return CreatedAtRoute("GetItem", new {itemId = itemToReturn.Id}, itemToReturn);
        }
        
        [HttpPost("{itemId}")]
        public IActionResult UpdateItem(Guid itemId, UpdateItemDto updateItemDto)
        {
            var itemToUpdate = _beachBuddyRepository.GetItem(itemId);
            if (itemToUpdate == null)
            {
                return NotFound();
            }

            _mapper.Map(updateItemDto, itemToUpdate);
            _beachBuddyRepository.UpdateItem(itemToUpdate);
            _beachBuddyRepository.Save();

            return NoContent();
        }
        
        [HttpDelete("{itemId}")]
        public IActionResult UpdateItem(Guid itemId)
        {
            var itemToDelete = _beachBuddyRepository.GetItem(itemId);
            if (itemToDelete == null)
            {
                return NotFound();
            }

            _beachBuddyRepository.DeleteItem(itemToDelete);
            _beachBuddyRepository.Save();

            return NoContent();
        }
    }
}