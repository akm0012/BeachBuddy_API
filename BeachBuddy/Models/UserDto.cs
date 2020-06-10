using System;
using System.Collections.Generic;
using BeachBuddy.Entities;

namespace BeachBuddy.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string FullName { get; set; }
        
        public List<ScoreDto> Scores { get; set; }
    }
}