namespace BeachBuddy.Models.Dtos.RequestedItem
{
    public class UpdateRequestedItemDto
    {
        public string Name { get; set; }

        public int Count { get; set; }
        
        public bool IsRequestCompleted { get; set; }
    }
}