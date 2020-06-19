using System;

namespace BeachBuddy.Models.Dtos.Score
{
    public class ScoreDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public int WinCount { get; set; }

        public Guid UserId { get; set; }
    }
}