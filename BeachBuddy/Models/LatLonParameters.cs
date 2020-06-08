using System.ComponentModel.DataAnnotations;
using BeachBuddy.ValidationAttributes;

namespace BeachBuddy.Models
{
    [LatLonValidation]
    public class LatLonParameters
    {
        [Required]
        public string Lat { get; set; }

        [Required]
        public string Lon { get; set; }
    }
}