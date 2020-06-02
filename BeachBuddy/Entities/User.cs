using System;
using System.ComponentModel.DataAnnotations;

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

        public int StarCount { get; set; }
        
        public int KanJamWinCount { get; set; }
    }
}