using System.Collections.Generic;

namespace BeachBuddy.Models
{
    public class SystemStatus
    {
        public bool IsDatabaseOk { get; set; }
        
        public bool IsBeachConditionsOk { get; set; }
        
        public bool IsWeatherOk { get; set; }
        
        public bool IsCurrentUvIndexOk { get; set; }

        public bool IsGetUsersOk { get; set; }
        
        public bool IsTwilioOk { get; set; }
        
        public string TwilioBalance { get; set; }
        
        public List<string> ErrorMessages { get; set; }
            = new List<string>();
    }
    
}