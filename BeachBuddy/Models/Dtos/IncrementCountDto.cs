using BeachBuddy.ValidationAttributes;

namespace BeachBuddy.Models.Dtos
{
    [IncrementCountFieldChecker]
    public class IncrementCountDto
    {
        // todo: could make enum and validation should be free
        public string AttributeName { get; set; }
        
        public int IncrementAmount { get; set; }
    }
}