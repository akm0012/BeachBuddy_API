using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BeachBuddy.Enums;

namespace BeachBuddy.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public SkinType SkinType { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string PhotoUrl { get; set; }
        
        public ICollection<Score> Scores { get; set; }
            = new List<Score>();
        
        public ICollection<RequestedItem> RequestedItems { get; set; }
            = new List<RequestedItem>();
    }
}