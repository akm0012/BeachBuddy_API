using System;
using System.ComponentModel.DataAnnotations;

namespace BeachBuddy.Models.Dtos.RequestedItem
{
    public class AddRequestedItemDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Count { get; set; }
        
        public Guid RequestedByUserId { get; set; }
    }
}