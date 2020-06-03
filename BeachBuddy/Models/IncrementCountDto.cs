using BeachBuddy.ValidationAttributes;

namespace BeachBuddy.Models
{
    [IncrementCountFieldChecker]
    public class IncrementCountDto
    {
        public string AttributeName { get; set; }
        
        public int IncrementAmount { get; set; }
    }
}