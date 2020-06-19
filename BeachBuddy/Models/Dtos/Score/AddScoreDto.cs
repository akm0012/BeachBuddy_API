using System.ComponentModel.DataAnnotations;

namespace BeachBuddy.Models.Dtos.Score
{
    public class AddScoreDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}