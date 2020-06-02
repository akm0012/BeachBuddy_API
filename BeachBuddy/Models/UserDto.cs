using System;

namespace BeachBuddy.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string FullName { get; set; }
        
        public int StarCount { get; set; }
        
        public string KanJamWinCount { get; set; } 
    }
}