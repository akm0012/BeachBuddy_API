using System;
using System.ComponentModel.DataAnnotations;

namespace BeachBuddy.Entities
{
    public class Item
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [MaxLength(1500)]
        public string Description { get; set; }
        
        public int Count { get; set; }
        
        public string ImageUrl { get; set; }
    }
}