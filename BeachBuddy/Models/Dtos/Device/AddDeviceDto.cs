using System.ComponentModel.DataAnnotations;
using BeachBuddy.Enums;

namespace BeachBuddy.Models.Dtos.Device
{
    public class AddDeviceDto
    {
        [Required]
        public string DeviceToken { get; set; }

        [Required]
        public DeviceType DeviceType { get; set; }
    }
}