using KantarAssessment.ShoppingBasketAPI.Model;

namespace KantarAssessment.ShoppingBasketAPI.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
    }
}