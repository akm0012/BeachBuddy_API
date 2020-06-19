using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BeachBuddy.Entities;
using BeachBuddy.Models;
using BeachBuddy.Models.Dtos.Device;
using BeachBuddy.Models.Dtos.RequestedItem;
using BeachBuddy.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BeachBuddy.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DeviceController : ControllerBase
    {
        private readonly IBeachBuddyRepository _beachBuddyRepository;
        private readonly IMapper _mapper;
        
        public DeviceController(IBeachBuddyRepository beachBuddyRepository, IMapper mapper)
        {
            _beachBuddyRepository = beachBuddyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceDto>>> GetRequestedItems()
        {
            return Ok(_mapper.Map<IEnumerable<DeviceDto>>(await _beachBuddyRepository.GetDevices()));
        }
        
        [HttpPost]
        public async Task<ActionResult> CreateDevice(AddDeviceDto addDeviceDto)
        {
            var itemToAdd = _mapper.Map<Device>(addDeviceDto);

            if (await _beachBuddyRepository.GetDevice(itemToAdd.DeviceToken) == null)
            {
                await _beachBuddyRepository.AddDevice(itemToAdd);
                await _beachBuddyRepository.Save();
            }

            return NoContent();
        }
    }
}