using System;
using System.Collections.Generic;
using BeachBuddy.Enums;
using BeachBuddy.Models.Dtos.Score;

namespace BeachBuddy.Models.Dtos.User
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string FullName { get; set; }
        
        public SkinType SkinType { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string PhotoUrl { get; set; }
        
        public List<ScoreDto> Scores { get; set; }
    }
}