using System;

namespace BeachBuddy.Models
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int Count { get; set; }
        
        public string ImageUrl { get; set; }
    }
}