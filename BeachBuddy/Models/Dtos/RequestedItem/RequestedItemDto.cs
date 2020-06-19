using System;
using BeachBuddy.Models.Dtos.User;

namespace BeachBuddy.Models.Dtos.RequestedItem
{
    public class RequestedItemDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public int Count { get; set; }
        
        public Guid? RequestedByUserId { get; set; }
        
        public UserDto RequestedByUser { get; set; }

        public bool IsRequestCompleted { get; set; }
        
        public DateTimeOffset CreatedDateTime { get; set; }

        public DateTimeOffset? CompletedDateTime { get; set; }
    }
}