using System;
using System.ComponentModel.DataAnnotations;

namespace BeachBuddy.Models
{
    public class AddScoreDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}