using System.Collections.Generic;

namespace BeachBuddy.Models.Dtos
{
    public class SystemStatusDto
    {
        public bool IsDatabaseOk { get; set; }
        
        public bool IsBeachConditionsOk { get; set; }
        
        public bool IsWeatherOk { get; set; }
        
        public bool IsCurrentUvIndexOk { get; set; }

        public bool IsGetUsersOk { get; set; }
        
        public string TwilioBalance { get; set; }
        
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}