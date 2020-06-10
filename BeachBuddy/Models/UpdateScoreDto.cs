using System.ComponentModel.DataAnnotations;

namespace BeachBuddy.Models
{
    public class UpdateScoreDto
    {
        [Required]
        public int WinCount { get; set; }
    }
}