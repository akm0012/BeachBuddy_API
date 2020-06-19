using System.ComponentModel.DataAnnotations;
using BeachBuddy.Models;
using BeachBuddy.Models.Dtos;

namespace BeachBuddy.ValidationAttributes

{
    public class IncrementCountFieldCheckerAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var incrementCountDto = (IncrementCountDto) validationContext.ObjectInstance;

            var attributeName = incrementCountDto.AttributeName;
            
            if (attributeName != "StarCount" && attributeName != "KanJamWinCount")
            {
                return new ValidationResult(
                    "Unknown attribute name. Must be either, 'StarCount' or 'KanJamWinCount'",
                    new [] {nameof(IncrementCountDto)});
            }
            
            return ValidationResult.Success;
        }         
    }
}