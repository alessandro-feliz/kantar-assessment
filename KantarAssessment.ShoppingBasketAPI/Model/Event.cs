namespace KantarAssessment.ShoppingBasketAPI.Model
{
    public class Event
    {
        public int EventId { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get { return StartDate <= DateTime.UtcNow && EndDate >= DateTime.UtcNow; } }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}