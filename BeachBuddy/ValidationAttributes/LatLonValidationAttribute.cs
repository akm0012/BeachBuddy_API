using System;
using System.ComponentModel.DataAnnotations;
using BeachBuddy.Models;

namespace BeachBuddy.ValidationAttributes
{
    public class LatLonValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var errorFound = false;
            var errorMessage = "Change in latitude, changes in attitude... ";
            
            var latLon = (LatLonParameters) validationContext.ObjectInstance;

            if (double.TryParse(latLon.Lat, out var lat))
            {
                if (lat < -90 || lat > 90)
                {
                    errorFound = true;
                    errorMessage += $"{lat} is not a valid Latitude. Must be between -90 and 90.";
                }
            }
            else
            {
                errorFound = true;
                errorMessage += $"{latLon.Lat} is not a number. Try again.";
            }

            if (!errorFound && double.TryParse(latLon.Lon, out var lon))
            {
                if (lon < -180 || lon > 180)
                {
                    errorFound = true;
                    errorMessage += $"{lon} is not a valid Longitude. Must be between -180 and 180.";
                }
            }
            else
            {
                errorFound = true;
                errorMessage += $"{latLon.Lon} is not a number. Try again.";
            }

            if (errorFound)
            {
                return new ValidationResult(
                    errorMessage,
                    new[] {nameof(LatLonParameters)});
            }
            
            return ValidationResult.Success;
        }
    }
}