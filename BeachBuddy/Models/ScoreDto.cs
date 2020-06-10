using System;

namespace BeachBuddy.Models
{
    public class ScoreDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public int WinCount { get; set; }

        public Guid UserId { get; set; }
    }
}