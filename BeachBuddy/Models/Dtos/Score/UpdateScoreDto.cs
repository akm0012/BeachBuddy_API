using System.ComponentModel.DataAnnotations;

namespace BeachBuddy.Models.Dtos.Score
{
    public class UpdateScoreDto
    {
        [Required]
        public int WinCount { get; set; }
    }
}