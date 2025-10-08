namespace KantarAssessment.ShoppingBasketAPI.Dto
{
    public class EventDto
    {
        public int EventId { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}