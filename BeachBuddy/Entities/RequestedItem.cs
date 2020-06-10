using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeachBuddy.Entities
{
    public class RequestedItem
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        public int Count { get; set; }
        
        public Guid RequestedByUserId { get; set; }
        
        public User RequestedByUser { get; set; }

        public bool IsRequestCompleted { get; set; }
        
        public DateTimeOffset CreatedDateTime { get; set; }
    }
}