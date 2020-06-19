using System.ComponentModel.DataAnnotations;
using BeachBuddy.Enums;

namespace BeachBuddy.Models.Dtos.User
{
    public class UpdateUserDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        
        public SkinType SkinType { get; set; }
    }
}