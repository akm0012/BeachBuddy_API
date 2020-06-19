using System;
using System.ComponentModel.DataAnnotations;
using BeachBuddy.Enums;

namespace BeachBuddy.Entities
{
    public class Device
    {
        [Key]
        public string DeviceToken { get; set; }
        
        public DeviceType DeviceType { get; set; }
    }
}