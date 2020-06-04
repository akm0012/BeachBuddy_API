using System.ComponentModel.DataAnnotations;

namespace BeachBuddy.Models
{
    public abstract class ManipulateItemDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [MaxLength(1500)]
        public string Description { get; set; }
        
        public int Count { get; set; }
        
        public string ImageUrl { get; set; } 
    }
}