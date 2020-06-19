using System.ComponentModel.DataAnnotations;
using BeachBuddy.Enums;

namespace BeachBuddy.Models
{
    public class AddDeviceDto
    {
        [Required]
        public string DeviceToken { get; set; }

        [Required]
        public DeviceType DeviceType { get; set; }
    }
}