using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeachBuddy.Entities
{
    public class Score
    {
        [Key]
        public Guid Id { get; set; }
 
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int WinCount { get; set; }
        
        // [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}